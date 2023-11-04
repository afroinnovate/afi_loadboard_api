using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Auth.API.Data;
using Auth.API.Dtos;
using AutoMapper;
using System.Security.Claims;

namespace Auth.API.Repository
{
    /// <summary>
    /// Provides an implementation for CRUD operations specific to the ApplicationUser entity,
    /// as well as additional operations related to user management.
    /// </summary>
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
    #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
    #endregion

    #region Constructor
        public UserRepository(ApplicationDbContext _dbSet, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        : base(_dbSet)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

    #endregion

    #region GetByUserName
        /// <summary>
        /// Asynchronously retrieves a user based on the username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>An instance of the UserDto representing the user, or null if no user matches the username.</returns>
        public async Task<UserDto> GetByUserNameAsync(string userName)
        {
            try
            {
                var user = await _dbset.Users
                                        .Where(u => u.UserName == userName)
                                        .FirstOrDefaultAsync();
                if (user != null)
                {
                    return _mapper.Map<UserDto>(user);
                }
                return new();
            }
            catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"An error occurred while getting user by username: {ex.Message}");
                throw;
            }
        }
    #endregion

    #region Register    
        /// <summary>
        /// Asynchronously registers a new user.
        /// </summary>
        /// <param name="registrationRequestDto">The data transfer object containing user registration details.</param>
        /// <returns>A string message indicating the result of the registration process.</returns>
        public async Task<ResponseDto> Register(ApplicationUser user, string password, string roleName)
        {
            try
            {
                user.DateRegistered = DateTime.UtcNow;
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    //Assign user to role
                    await AssignRole(user, roleName);

                    return new ResponseDto()
                    {
                        Message = "Registration successful, userId: " + user.Id + " and assigned Role: " + roleName,
                        IsSuccess = true
                    };
                }
                return new ResponseDto()
                {
                    Message = "Registration Failed, userId: "+user.UserName,
                    IsSuccess = false,
                    Result = result.Errors
                };
                
            }
            catch (Exception ex)
            {
                // log the exception
                throw (ex);
            }
        }

        /// <summary>
        /// Asynchronously logs a user in.
        /// </summary>
        /// <param name="loginRequestDto">The data transfer object containing user login details.</param>
        /// <returns>An instance of the LoginResponseDto containing the login result and related data.</returns>
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                ApplicationUser? user = string.IsNullOrEmpty(loginRequestDto.UserName) ?
                    _dbset.ApplicationUsers.FirstOrDefault(user => user.Email == loginRequestDto.Email) :
                    _dbset.ApplicationUsers.FirstOrDefault(user => user.UserName == loginRequestDto.UserName);

                if (user == null)
                {
                    return new();
                }
                
                // var emailResult = await _userManager.CheckPasswordAsync(new ApplicationUser() { Emi}, loginRequestDto.Password);
                var result = await _signInManager.PasswordSignInAsync(user, loginRequestDto.Password, false, false);

                if (!result.Succeeded)
                {
                    return new();
                }

                return new LoginResponseDto
                {
                    IsLockedOut = result.IsLockedOut,
                    RequiresTwoFactor = result.RequiresTwoFactor,
                    User = new()
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateRegistered = user.DateRegistered,
                        DateLoggedIn = user.DateLastLoggedIn?? DateTime.Now
                    }
                };
            }
            catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"An error occurred while logging in: {ex.Message}");
                throw;
            }
        }
    #endregion

    #region Logout
        /// <summary>
        /// Asynchronously logs a user out.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task Logout()
        {
            try
            {
                // Sign out the current user
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred while logging out: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    #endregion

    #region AssignRole
        /// <summary>
        /// Asynchronously assigns a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the role should be assigned.</param>
        /// <param name="roleName">The name of the role to assign to the user.</param>
        /// <returns>A boolean indicating whether the role was successfully assigned to the user.</returns>
        public async Task<bool> AssignRole(ApplicationUser user, string roleName)
        {
            try
            {
                //Add the role if the role doesn't already exists
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create the role
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // assign the role to the user
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error assigning role to user with ID {user.UserName}: {e.Message}");
            }
        }
    #endregion

    #region RemoveRole
        /// <summary>
        /// Asynchronously removes a role from a user.
        /// </summary>
        /// <param name="user">The user from whom the role should be removed.</param>
        /// <param name="roleName">The name of the role to remove from the user.</param>
        /// <returns>A boolean indicating whether the role was successfully removed from the user.</returns>
        public async Task<bool> RemoveRole(ApplicationUser user, string roleName)
        {
            try
            {
                // Check if the user has the role
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    // Remove the role from the user
                    var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                    // Return true if the role was successfully removed, otherwise false
                    return result.Succeeded;
                }

                // If the user does not have the role, return true as there's nothing to remove
                return true;
            }
            catch (Exception e)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error removing role from user with ID {user.UserName}: {e.Message}");
            }
        }
    #endregion

    #region GetUserRole
        /// <summary>
        /// Asynchronously retrieves a user role based on ID.
        /// </summary>  
        /// <param name="userId">The ID of the user whose role should be retrieved.</param>
        /// <returns>The name of the user's role.</returns>
        public async Task<List<string>> GetUserRole(string userId)
        {
            try
            {

                var user = userId.Contains('@')? await _userManager.FindByEmailAsync(userId) : await _userManager.FindByNameAsync(userId);
                if (user == null)
                {
                    return new List<string>();
                }
                else
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return roles.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving user role for user with ID {userId}: {e.Message}");
            }
        }
    #endregion

    #region GeneratePasswordResetToken
        /// <summary>
        /// Asynchronously generates a password reset token for a user.
        /// </summary>
        /// <param name="email">The email of the user to generate a reset token for.</param>
        /// <returns>A token that can be used to reset the user's password.</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new InvalidOperationException("User does not exist.");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return token;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    #endregion

    #region ResetPassword
        /// <summary>
        /// Asynchronously resets a user's password using a password reset token.
        /// </summary>
        /// <param name="email">The email of the user resetting their password.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password for the user.</param>
        /// <returns>A ResponseDto indicating the result of the password reset process.</returns>
        public async Task<ResponseDto> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ResponseDto
                {
                    Message = "User does not exist.",
                    IsSuccess = false
                };

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Password reset successful.",
                    IsSuccess = true
                };
            }
            else
            {
                return new ResponseDto
                {
                    Message = "Password reset failed.",
                    IsSuccess = false,
                    Result = result.Errors
                };
            }
        }
        #endregion
    }
}