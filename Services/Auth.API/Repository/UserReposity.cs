using Microsoft.EntityFrameworkCore;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Auth.API.Data;
using Auth.API.Dtos;
using AutoMapper;

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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext _dbSet, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        : base(_dbSet)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

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
                    await AssignRole(user.Id, roleName);

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
                var username = string.IsNullOrEmpty(loginRequestDto.UserName) ? loginRequestDto.Email : loginRequestDto.UserName;
                var result = await _signInManager.PasswordSignInAsync(username, loginRequestDto.Password, false, false);

                if (!result.Succeeded)
                {
                    return new();
                }

                var user = username.Contains("@") ? _dbset.ApplicationUsers.FirstOrDefault(user => user.Email == username) :
                                                    _dbset.ApplicationUsers.FirstOrDefault(user => user.UserName.ToLower() == username);
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

        /// <summary>
        /// Asynchronously assigns a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the role should be assigned.</param>
        /// <param name="roleName">The name of the role to assign to the user.</param>
        /// <returns>A boolean indicating whether the role was successfully assigned to the user.</returns>
        public async Task<bool> AssignRole(string userId, string roleName)
        {
            try
            {
                 // find the user first either by email or by id
                var user = userId.Contains('@') ? _userManager.FindByEmailAsync(userId).GetAwaiter().GetResult(): 
                                                _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                if (user == null)
                {
                    return false;
                }
                else
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
            }
            catch (Exception e)
            {
                throw new Exception($"Error assigning role to user with ID {userId}: {e.Message}");
            }
        }

        /// <summary>
        /// Asynchronously retrieves a user role based on ID.
        /// </summary>  
        /// <param name="userId">The ID of the user whose role should be retrieved.</param>
        /// <returns>The name of the user's role.</returns>
        public async Task<List<string>> GetUserRole(string userId)
        {
            try
            {

                var user = userId.Contains('@')? await _userManager.FindByEmailAsync(userId) :
                                                 await _userManager.FindByNameAsync(userId);
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
    }
}