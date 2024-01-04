
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
    /// <summary>
    /// The name of the authentication endpoint group.
    /// </summary>
    private const string GetAuthEndpointName = "auth";

    /// <summary>
    /// Adds registration endpoints to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    /// <param name="logger">The logger.</param>
    public static void AddRegistrationEndpoints(this RouteGroupBuilder group, ILogger logger)
    {
            
        group.MapPost("/register", async (RegistrationModel model, UserManager<AppUser> userManager, IEmailConfigService emailService) =>
        { 
            logger.LogInformation("creating user");
            // Handle user registration logic here
            var user = new AppUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                logger.LogError($"error creating user: {errors}");
                return Results.BadRequest(new { Errors = errors });
            }

            try
            {
                logger.LogInformation("sending email with token");
                // Generate email confirmation token
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create a confirmation link (you would replace "http://localhost:5000" with your front-end URL)
                var confirmationLink = $"http://app.loadboard.afroinnovate.com/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

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
                return Results.Problem("The user was created but there was an issue sending the confirmation email.");
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
        group.MapPost("/completeprofile", async (CompleteProfileRequest request, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) => 
        {
            if (request.Username == null)
            {
                return Results.BadRequest("Username is required");
            }

            // Find the user
            var user = await userManager.FindByEmailAsync(request.Username);
            if (user == null)
            {
                return Results.NotFound("User not Found");
            }

            if (request.FirstName != null || request.LastName != null || request.DotNumber != null || request.CompanyName != null)
            {
                // Update user details
                user.FirstName = string.IsNullOrEmpty(request.FirstName) || request.FirstName == "string" ? user.FirstName : request.FirstName;
                user.LastName = string.IsNullOrEmpty(request.LastName) || request.LastName == "string" ? user.LastName : request.LastName;
                user.DotNumber = string.IsNullOrEmpty(request.DotNumber) || request.DotNumber == "string" ? user.DotNumber : request.DotNumber;
                user.CompanyName = string.IsNullOrEmpty(request.CompanyName) || request.CompanyName == "string" ? user.CompanyName : request.CompanyName;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return Results.BadRequest(updateResult.Errors);
                }
            }

            // Assign the role to the user
            if (!string.IsNullOrEmpty(request.Role))
            {
                // check if the roles is valid 
                // Check if the role is in the predefined roles
                if (!IsRoleValid(request.Role, roleConfig))
                {
                    return Results.BadRequest("Invalid role.");
                }

                // Check if the role exists in the database, create if not
                if (!await roleManager.RoleExistsAsync(request.Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(request.Role));
                }

                // Check if the role is valid
                if (!await roleManager.RoleExistsAsync(request.Role) )
                {
                    return Results.BadRequest("Invalid role.");
                }

                var roleResult = await userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    return Results.BadRequest(roleResult.Errors);
                }
            }

            return Results.Ok("User profile updated successfully");
        }).WithName("CompleteProfile")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Adds the login endpoint to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    public static void AddLoginEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (LoginRequest model, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtTokenGenerator tokenGenerator) =>
        {
            if ((string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password)) || (model.Username == "string" || model.Password == "string"))
            {
                return Results.BadRequest("Invalid username or password");
            }

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null || !(await signInManager.CheckPasswordSignInAsync(user, model.Password, false)).Succeeded)
            {
                return Results.Unauthorized();
            }

            var token = tokenGenerator.GenerateToken(user, await userManager.GetRolesAsync(user));

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
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Roles = await userManager.GetRolesAsync(user)
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
        groups.AddRegistrationEndpoints(logger);
        groups.AddLoginEndpoint();
        groups.AddCompleteProfileEndpoint(rolesConfig);

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
            var  mailMessage = new MimeMessage();
        
            mailMessage.From.Add(MailboxAddress.Parse(fromEmail));
            mailMessage.Subject = request.Subject;
            mailMessage.Body = new TextPart(TextFormat.Html){Text = request.Body};
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