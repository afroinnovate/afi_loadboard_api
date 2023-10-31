using Microsoft.EntityFrameworkCore;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Auth.API.Data;
using Auth.API.Dtos;

namespace Auth.API.Repository
{
    /// <summary>
    /// Provides an implementation for CRUD operations specific to the ApplicationUser entity,
    /// as well as additional operations related to user management.
    /// </summary>
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext _dbSet, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        : base(_dbSet)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Asynchronously retrieves a user based on the username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>An instance of the UserDto representing the user, or null if no user matches the username.</returns>
        public async Task<UserDto> GetByUserNameAsync(string userName)
        {
            var user = await _dbset.Users
                                    .Where(u => u.UserName == userName)
                                    .FirstOrDefaultAsync();
            if (user != null)
            {
                return new UserDto
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }
            return new();
        }

        /// <summary>
        /// Asynchronously registers a new user.
        /// </summary>
        /// <param name="registrationRequestDto">The data transfer object containing user registration details.</param>
        /// <returns>A string message indicating the result of the registration process.</returns>
        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            var user = new ApplicationUser
            {
                UserName = string.IsNullOrEmpty(registrationRequestDto.UserName)? registrationRequestDto.UserName : "",
                Email = registrationRequestDto.Email,
                DateRegistered = DateTime.Now,
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                MiddleName = string.IsNullOrEmpty(registrationRequestDto.MiddleName)? registrationRequestDto.MiddleName: null,
                PhoneNumber = string.IsNullOrEmpty(registrationRequestDto.PhoneNumber)? registrationRequestDto.PhoneNumber : ""
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            return result.Succeeded ? "Registration successful" : "Registration failed";
        }

        /// <summary>
        /// Asynchronously logs a user in.
        /// </summary>
        /// <param name="loginRequestDto">The data transfer object containing user login details.</param>
        /// <returns>An instance of the LoginResponseDto containing the login result and related data.</returns>
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var username = string.IsNullOrEmpty(loginRequestDto.UserName) ? loginRequestDto.Email : loginRequestDto.UserName;
            var result = await _signInManager.PasswordSignInAsync(username, loginRequestDto.Password, false, false);

            if (!result.Succeeded)
            {
                return new();
            }

            return new LoginResponseDto
            {
                IsLockedOut = result.IsLockedOut,
                RequiresTwoFactor = result.RequiresTwoFactor,
            };
        }

        /// <summary>
        /// Asynchronously assigns a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the role should be assigned.</param>
        /// <param name="roleName">The name of the role to assign to the user.</param>
        /// <returns>A boolean indicating whether the role was successfully assigned to the user.</returns>
        public async Task<bool> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            return false;
        }

    }
}