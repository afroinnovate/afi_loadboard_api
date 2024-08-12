
using System.Security.Claims;
using Auth.Min.API.Dtos;
using Auth.Min.API.Models;
using Auth.Min.API.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MimeKit;
using MimeKit.Text;


/// <summary>
/// Contains the endpoints related to authentication and user management.
/// </summary>
namespace Auth.Min.API.Endpoints;

/// <summary>
/// Provides methods to add registration endpoints.
/// </summary>
public static class AuthEndpoints
{
    // Nested non-static class for logging purposes
    private class LogCategory
    {
        // This class is intentionally left empty and is just used as a category for logging
    }


    /// <summary>
    /// The name of the authentication endpoint group.
    /// </summary>
    private const string GetAuthEndpointName = "auth";

    /// <summary>
    /// Adds registration endpoints to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    /// <param name="logger">The logger.</param>
    public static void AddRegistrationEndpoints(this RouteGroupBuilder group, Roles roleConfig)
    {

        group.MapPost("/register", async (RegistrationModel request, UserManager<AppUser> userManager, IEmailConfigService emailService, RoleManager<IdentityRole> roleManager, ILogger<LogCategory> logger) =>
        {
            logger.LogInformation("creating user");
            // Handle user registration logic here
            var user = new AppUser { UserName = request.Email, Email = request.Email };
            try
            {
                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    logger.LogError("error creating user: {error}", result.Errors);
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    logger.LogError($"error creating user: {errors}");
                    return Results.BadRequest(new { Errors = errors });
                }

                #region Update User Role
                logger.LogInformation("User details updated successfully for user {Username}", request.Email);
                // // Assign the role to the user
                // if (request.UserType != null)
                // {
                //     logger.LogInformation("Assigning role {Role} to user {Username}", request.UserType, request.Email);
                //     // check if the roles is valid 
                //     // Check if the role is in the predefined roles
                //     if (!IsRoleValid(request.UserType, roleConfig))
                //     {
                //         logger.LogWarning("Invalid role {Role}", request.UserType);
                //         return Results.BadRequest("Invalid role.");
                //     }

                //     // Check if the role exists in the database, create if not
                //     if (!await roleManager.RoleExistsAsync(request.UserType))
                //     {
                //         logger.LogInformation("Role {Role} does not exist, creating it", request.UserType);
                //         await roleManager.CreateAsync(new IdentityRole(request.UserType));
                //     }

                //     // Check if the role is valid
                //     if (!await roleManager.RoleExistsAsync(request.UserType))
                //     {
                //         logger.LogWarning("Invalid role {Role}", request.UserType);
                //         return Results.BadRequest("Invalid role.");
                //     }

                //     logger.LogInformation("Assigning role {Role} to user {Username}", request.UserType, request.Email);
                //     var roleResult = await userManager.AddToRoleAsync(user, request.UserType);
                //     if (!roleResult.Succeeded)
                //     {
                //         logger.LogError("Error assigning role {Role} to user {Username}", request.UserType, request.Email);
                //         return Results.BadRequest(roleResult.Errors);
                //     }
                // }
                #endregion

                logger.LogInformation("sending email with token");
                // Generate email confirmation token
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create a confirmation link (you would replace "http://localhost:5000" with your front-end URL)
                var confirmationLink = $"http://app.loadboard.afroinnovate.com/confirm-email?userId={user.Id}";

                // var tempConfirmationLink = $"http://localhost:3000/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
                logger.LogInformation("Attempting to send confirmation email to {Email}", user.Email);

                // Send email
                var emailRequest = new EmailRequestDto
                {
                    Subject = "Welcome to Loadboard - Confirm Your Registration",
                    Body = "Dear " + user.UserName + ",<br/><br/>" +
                        "Welcome to Loadboard, Africa’s premier transport network connecting the continent’s vibrant economies and communities. We are thrilled to have you join our growing family of users who are driving change and fostering connections across Africa.<br/><br/>" +
                        "To complete your registration and embark on this transformative journey with us, please <a href='" + confirmationLink + "'>*******confirm your account using this Link - CLICK ME****</a>.<br/><br/>" +
                        "Once your account is confirmed, you will have full access to our platform’s features and begin to experience the convenience and efficiency that Loadboard offers.<br/><br/>" +
                        "We are committed to providing you with exceptional service and a seamless experience. Should you have any questions or need assistance, please do not hesitate to reach out to our support team on <i>support@afroinnovate.com<i>.<br/><br/>" +
                        "Thank you for choosing Loadboard. Together, let’s move Africa forward.<br/><br/>" +
                        "Warm regards,<br/>" +
                        "The Loadboard Team",
                    To = user.Email
                };


                var emailResult = await SendEmailAsync(emailRequest, logger, emailService);
                if (!emailResult)
                {
                    logger.LogError($"error sending email, deleting this wrong user from database: {emailResult}");
                    await userManager.DeleteAsync(user);
                    return Results.Problem("The user was created but there was an issue sending the confirmation email.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"error sending email: {ex.Message}");
                // Log the error, handle it, or inform the user as necessary
                return Results.BadRequest("The user was created but there was an issue sending the confirmation email.");
            }
            // You might want to check the emailResult for specific error messages
            logger.LogInformation("Confirmation email sent to {Email}", user.Email);
            return Results.Ok("User created successfully, confirmation email is sent to the user.");
        })
        .WithName("RegisterUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Adds the complete profile endpoint to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    /// <param name="roleConfig">The role configuration.</param>
    public static void AddCompleteProfileEndpoint(this RouteGroupBuilder group, Roles roleConfig)
    {
        group.MapPut("/completeprofile", async (CompleteProfileRequest request, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<LogCategory> logger) =>
        {
            logger.LogInformation("Completing profile for user {Email}", request.Email);
            if (request.Email == null)
            {
                logger.LogWarning("Email or Username is required");
                return Results.BadRequest("Email is required");
            }

            try
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                logger.LogInformation("User found with email {User}", user);
                if (user == null)
                {
                    logger.LogWarning("User not found");
                    return Results.NotFound("User not Found");
                }

                if (request.FirstName != null || request.LastName != null || request.PhoneNumber != null)
                {
                    logger.LogInformation("Updating user details for user {Username}", request.Email);
                    // Update user details
                    user.FirstName = string.IsNullOrEmpty(request.FirstName) || request.FirstName == "" ? user.FirstName : request.FirstName;
                    user.LastName = string.IsNullOrEmpty(request.LastName) || request.LastName == "" ? user.LastName : request.LastName;
                    user.MiddleName = string.IsNullOrEmpty(request.MiddleName) || request.MiddleName == "" ? user.MiddleName : request.MiddleName;
                    user.PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) || request.PhoneNumber == "" ? user.PhoneNumber : request.PhoneNumber;
                    user.EmailConfirmed = true;
                    user.Confirmed = true;
                    user.Status = request.Status;
                    user.UserType = request.UserType;

                    var updateResult = await userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        return Results.BadRequest(updateResult.Errors);
                    }
                }

                #region Update User Role
                logger.LogInformation("User details updated successfully for user {Username}", request.Email);
                // Assign the role to the user
                if (!string.IsNullOrEmpty(request.Role))
                {
                    logger.LogInformation("Assigning role {Role} to user {Username}", request.Role, request.Email);
                    // check if the roles is valid 
                    // Check if the role is in the predefined roles
                    if (!IsRoleValid(request.Role, roleConfig))
                    {
                        logger.LogWarning("Invalid role {Role}", request.Role);
                        return Results.BadRequest("Invalid role.");
                    }

                    // Check if the role exists in the database, create if not
                    if (!await roleManager.RoleExistsAsync(request.Role))
                    {
                        logger.LogInformation("Role {Role} does not exist, creating it", request.Role);
                        await roleManager.CreateAsync(new IdentityRole(request.Role));
                    }

                    // Check if the role is valid
                    if (!await roleManager.RoleExistsAsync(request.Role))
                    {
                        logger.LogWarning("Invalid role {Role}", request.Role);
                        return Results.BadRequest("Invalid role.");
                    }

                    logger.LogInformation("Assigning role {Role} to user {Username}", request.Role, request.Email);
                    var roleResult = await userManager.AddToRoleAsync(user, request.Role);
                    if (!roleResult.Succeeded)
                    {
                        logger.LogError("Error assigning role {Role} to user {Username}", request.Role, request.Email);
                        return Results.BadRequest(roleResult.Errors);
                    }
                }
                #endregion

                logger.LogInformation("User profile updated successfully for user {Username}", request.Email);
                return Results.Ok("User profile updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error updating user details: {ex.Message}");
                return Results.BadRequest("Error updating user details");
            }

        }).WithName("CompleteProfile")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// This request endpoint for password reset.
    /// </summary>
    /// <param name="group"></param> <summary>
    ///
    /// </summary>
    /// <param name="group"></param>
    public static void AddPasswordResetEndpoints(this RouteGroupBuilder group)
    {
        // Endpoint to request password reset
        group.MapPost("/request-password-reset", async (string email, UserManager<AppUser> userManager, IEmailConfigService emailService, ILogger<LogCategory> logger) =>
        {
            logger.LogInformation("Requesting password reset for {0}", email);
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogWarning("Password reset requested for non-existing user.");
                return Results.NotFound("User does not exist.");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"https://app.loadboard.afroinnovate.com/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

            // Send email with reset link
            var emailResult = await SendEmailAsync(new EmailRequestDto
            {
                Subject = "Reset Your Password",
                Body = $"Please click on the link to reset your password: <a href='{resetLink}'>Reset Password</a>",
                To = user.Email
            }, logger, emailService);

            if (!emailResult)
            {
                logger.LogError("Failed to send password reset email.");
                return Results.Problem("Failed to send password reset email.");
            }
            logger.LogInformation("Password reset link sent to {0}", email);
            return Results.Ok("Password reset link sent to your email.");
        })
        .WithName("RequestPasswordReset")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // Endpoint to reset the password
        group.MapPost("/reset-password", async (ResetPasswordModelDTO model, UserManager<AppUser> userManager, ILogger<LogCategory> logger) =>
        {
            logger.LogInformation("Reseting password for {0}", model.Email);
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                logger.LogWarning("Password reset requested for non-existing user, make sure you have an existing account.");
                return Results.NotFound("User does not exist.");
            }

            var tokenValid = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", model.Token);
            if (!tokenValid)
            {
                logger.LogWarning("Invalid password reset token for {0}", model.Email);
                return Results.BadRequest("Invalid or expired token.");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                logger.LogError("Bad request, resetting password: {0}", result.Errors);
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Results.BadRequest(new { Errors = errors });
            }
            logger.LogInformation("Password reset successfully for {0}", model.Email);
            return Results.Ok("Password has been reset successfully.");
        })
        .WithName("ResetPassword")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/update-password-inprofile", async (UpdatePasswordDTO model, UserManager<AppUser> userManager, ILogger<LogCategory> logger) =>
        {
            logger.LogInformation("Updating the password from profile for {0}", model.Email);
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                logger.LogWarning("Password update requested for non-existing user.");
                return Results.NotFound("User does not exist.");
            }

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                logger.LogError("Bad request, updating password: {0}", result.Errors);
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Results.BadRequest(new { Errors = errors });
            }
            logger.LogInformation("Password updated successfully for {0}", model.Email);
            return Results.Ok("Password has been updated successfully.");
        })
        .WithName("UpdatePassword")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }


    /// <summary>
    /// Adds the login endpoint to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    public static void AddLoginEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (LoginRequest model, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtTokenGenerator tokenGenerator, ILogger<LogCategory> logger) =>
        {
            logger.LogInformation("Login attempt for user {Username}", model.Username);

            if ((string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password)) || (model.Username == "string" || model.Password == "string"))
            {
                logger.LogWarning("Invalid login attempt: Username or password is missing");
                return Results.BadRequest("Invalid username or password");
            }

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null || !(await signInManager.CheckPasswordSignInAsync(user, model.Password, false)).Succeeded)
            {
                logger.LogWarning("Invalid login attempt: User {Username} not found", model.Username);
                return Results.Unauthorized();
            }

            if (!user.EmailConfirmed)
            {
                logger.LogWarning("Login attempt for unconfirmed user {Username}", model.Username);
                return Results.BadRequest("User profile is not confirmed, please check your email and confirm it.");
            }

            var token = tokenGenerator.GenerateToken(user, await userManager.GetRolesAsync(user));
            logger.LogInformation("User {Username} logged in successfully", model.Username);

            // You would also generate a refresh token here and save it to the database or a cache

            return Results.Ok(new LoginResponse
            {
                Token = token,
                IsLockedOut = user.LockoutEnabled,
                RequiresTwoFactor = user.TwoFactorEnabled,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = await userManager.GetRolesAsync(user),
                    Confirmed = user.Confirmed,
                    Status = user.Status,
                    UserType = user.UserType
                },
                // refreshToken = "Your generated refresh token here"
            });
        })
        .WithName("Login")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Adds the get all users endpoint to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="logger">The logger.</param>
    public static void AddGetAllUsersEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/users", async (UserManager<AppUser> userManager, ILogger<LogCategory> logger) =>
        {
            // Fetch all users with their details
            var users = userManager.Users.ToList();

            // Create a list to hold the data transfer objects
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                // For each user, get their roles
                var roles = await userManager.GetRolesAsync(user);

                // Create and fill the user DTO
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    PhoneNumber = user.PhoneNumber,
                    Confirmed = user.EmailConfirmed,
                    Status = user.Status,
                    UserType = user.UserType,
                });
            }

            logger.LogInformation("Fetched all users");
            return Results.Ok(userDtos);
        })
        .WithName("GetAllUsers")
        .Produces<List<UserDto>>(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Maps the authentication endpoints to the specified route builder.
    /// </summary>
    /// <param name="routes">The endpoint route builder.</param>
    /// <param name="rolesConfig">The roles configuration.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>The route group builder.</returns>
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes, Roles rolesConfig, ILogger logger)
    {
        var groups = routes.MapGroup("/auth").WithName(GetAuthEndpointName);

        groups.MapGet("/Health", () => "OK from Auth.Min.API");

        // Add registration and complete profile endpoints
        groups.AddRegistrationEndpoints(rolesConfig);
        groups.AddLoginEndpoint();
        groups.AddCompleteProfileEndpoint(rolesConfig);
        groups.AddPasswordResetEndpoints(); // Include the password reset endpoints
        groups.AddGetAllUsersEndpoint(); // Include the endpoint to view all users

        return groups;
    }

    /// <summary>
    /// Checks if the specified role is valid.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <param name="rolesConfig">The roles configuration.</param>
    /// <returns><c>true</c> if the role is valid; otherwise, <c>false</c>.</returns>
    private static bool IsRoleValid(string role, Roles rolesConfig)
    {
        var roleProperties = rolesConfig.GetType().GetProperties();
        return roleProperties.Any(prop => string.Equals((string?)prop.GetValue(rolesConfig), role, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="request">The email request.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="emailService">The email configuration service.</param>
    /// <returns><c>true</c> if the email is sent successfully; otherwise, <c>false</c>.</returns>
    private static async Task<bool> SendEmailAsync(EmailRequestDto request, ILogger logger, IEmailConfigService emailService)
    {
        try
        {
            var (client, fromEmail) = await emailService.EmailSetup();

            logger.LogInformation("Creating email message");
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(MailboxAddress.Parse(fromEmail));
            mailMessage.Subject = request.Subject;
            mailMessage.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            mailMessage.To.Add(MailboxAddress.Parse(request.To));

            await client.SendAsync(mailMessage);
            await client.DisconnectAsync(true);

            logger.LogInformation("Email sent successfully");
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while sending email: " + ex.Message);
        }
    }
}