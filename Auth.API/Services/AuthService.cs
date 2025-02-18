using System.Net.Mime;
using Auth.API.Dtos;
using Auth.API.Models;
using Auth.API.Repository;
using Auth.API.Services.IServices;
using AutoMapper;

/// <summary>
/// This class implements the IAuthService interface and provides methods for assigning and removing roles for users, as well as logging in users.
/// </summary>
namespace Auth.API.Services
{
    class AuthService : IAuthService
    {
    #region Fields
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
    #endregion

    #region Constructor
        public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, 
        ILogger<AuthService> logger, IMapper mapper)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }
    #endregion

    #region AssignRole
        public async Task<bool> AssignRole(string userId, string roleName)
        {
            try
            {
                var userdto = await _userRepository.GetByUserNameAsync(userId);

                //check if user is registered
                if (userdto == null)
                {
                    _logger.LogError($"User with ID {userId} not found.");
                    throw (new ArgumentNullException());
                }

                //convert userDto to ApplicationUser
                var user =  new ApplicationUser
                {
                    Email = userdto.Email,
                    FirstName = userdto.FirstName,
                    LastName = userdto.LastName,
                    MiddleName = userdto.MiddleName,
                    PhoneNumber = userdto.PhoneNumber,
                    UserName = userdto.UserName
                }; 

                //assign role
                var result = await _userRepository.AssignRole(user, roleName);

                if (!result)
                {
                    _logger.LogError($"Error assigning role {roleName} to user with ID {userId}.");
                    return false;
                }
                _logger.LogInformation($"Role {roleName} assigned to user with ID {userId}.");
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error assigning role to user with ID {userId}: {e.Message}");
                throw (new Exception(
                    "Error happened: " + e
                ));
            }
        }
    #endregion

    #region RemoveRole
        public async Task<bool> RemoveRole(string userId, string roleName)
        {
            try
            {
                var userdto = await _userRepository.GetByUserNameAsync(userId);
                //check if user is registered
                if (userdto == null)
                {
                    _logger.LogError($"User with ID {userId} not found.");
                    throw (new ArgumentNullException());
                }

                //convert userDto to ApplicationUser
                var user = new ApplicationUser
                {
                    Email = userdto.Email,
                    FirstName = userdto.FirstName,
                    LastName = userdto.LastName,
                    MiddleName = userdto.MiddleName,
                    PhoneNumber = userdto.PhoneNumber,
                    UserName = userdto.UserName
                };

                //Remove the role
                var result = await _userRepository.RemoveRole(user, roleName);

                if (!result)
                {
                    _logger.LogError($"Error Removing role {roleName} to user with ID {userId}.");
                    throw new Exception();
                }
                _logger.LogInformation($"Role {roleName} assigned to user with ID {userId}.");
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error assigning role to user with ID {userId}: {e.Message}");
                throw new Exception(e.Message);
            }
        }
    #endregion

    #region Login
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, string loginType)
        {
            if (loginRequestDto == null)
            {
                _logger.LogError("Login request data transfer object is null.");
                return Task.FromResult(new LoginResponseDto());
            }

            try
            {
                //Login user
                var result = _userRepository.Login(loginRequestDto).Result;
                if (result.User == null)
                {
                    _logger.LogError($"Error logging in user with username {loginRequestDto.UserName}.");
                    return Task.FromResult(new LoginResponseDto());
                }
                else 
                {

                    var username = loginType == "username" ? loginRequestDto.UserName : loginRequestDto.Email;
                    //Get the user's role
                    var roles = _userRepository.GetUserRole(username).Result;
                    
                    if (roles.Count == 0)
                    {
                        _logger.LogError($"Error retrieving role for user with username {loginRequestDto.UserName}.");
                        return Task.FromResult(new LoginResponseDto());
                    }

                    //map userDto to applicationUser mapping doesn't work currently
                    // var user = _mapper.Map<ApplicationUser>(result.User);
                    var user  = new ApplicationUser
                    {
                        Email = loginType.Equals("username") ? null : username,
                        FirstName = result.User.FirstName?? null,
                        LastName = result.User.LastName?? null,
                        MiddleName = result.User.MiddleName?? null,
                        PhoneNumber = result.User.PhoneNumber?? null,
                        UserName = loginType.Equals("email") ? null : username,
                    };

                    //Generate token
                    var token = _jwtTokenGenerator.GenerateToken(user, roles);
                    if (string.IsNullOrEmpty(token))
                    {
                        _logger.LogError($"Error generating token for user with username {loginRequestDto.UserName}.");
                        return Task.FromResult(new LoginResponseDto());
                    }

                    //Return login response
                    UserDto userDto = new()
                    {
                        Email = loginType.Equals("username")? null : username,
                        FirstName = result.User.FirstName,
                        LastName = result.User.LastName,
                        MiddleName = result.User.MiddleName,
                        PhoneNumber = result.User.PhoneNumber,
                        UserName = loginType.Equals("email") ? null : username,
                    };

                    _logger.LogInformation($"User with username {loginRequestDto.UserName} logged in successfully.");
                    return Task.FromResult(new LoginResponseDto
                    {
                        Token = token,
                        User = userDto
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error logging in user with username {loginRequestDto.UserName}: {e.Message}");
                throw (new Exception(e.Message));
            }
        }
    #endregion    

    #region Logout
        public Task logout()
        {
            try
            {
                _userRepository.Logout();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error logging out user: {e.Message}");
                throw (new Exception(e.Message));
            }
        }
    #endregion 

    #region Registration
        public async Task<ResponseDto> Register(RegistrationRequestDto registerationRequestDto, string roleName)
        {
            if (registerationRequestDto == null || registerationRequestDto.Password == null)
            {
                _logger.LogError("Registration request data or transfer object is null or password.");
                return new ResponseDto()
                {
                    Message = "Registration request data or transfer object is null or password",
                    IsSuccess = false,
                };
            }
            try
            {
                // var appUserDto = _mapper.Map<ApplicationUser>(registerationRequestDto);
                var appUser = new ApplicationUser
                {
                    Email = registerationRequestDto.Email,
                    FirstName = registerationRequestDto.FirstName,
                    LastName = registerationRequestDto.LastName,
                    MiddleName = registerationRequestDto.MiddleName,
                    PhoneNumber = registerationRequestDto.PhoneNumber,
                    UserName = registerationRequestDto.UserName
                };
                
                var response = await _userRepository.Register(appUser, registerationRequestDto.Password, roleName);

                const string successMessage = "User with username {0} registered successfully.";
                const string errorMessage = "Error registering user with username {0}.";

                if (response.IsSuccess)
                {
                    _logger.LogInformation(successMessage, registerationRequestDto.UserName);
                    return response;
                }
                else
                {
                    _logger.LogError(errorMessage, registerationRequestDto.UserName);
                    return response;
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Error registering user with username {registerationRequestDto.UserName}: {e.Message}");
                throw new Exception(e.Message);
            }
        }
    #endregion

    #region ResetPassword
        public async Task<ResponseDto> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var response = await _userRepository.ResetPasswordAsync(email, token, newPassword);
                if (response.IsSuccess)
                {
                    _logger.LogInformation($"Password reset for user with email {email} was successful.");
                    return response;
                }
                else
                {
                    _logger.LogError($"Error resetting password for user with email {email}.");
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error resetting password for user with email {email}: {e.Message}");
                throw new Exception(e.Message);
            }
        }
    #endregion

    #region GeneratePasswordResetToken
        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                var token = await _userRepository.GeneratePasswordResetTokenAsync(email);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError($"Error generating password reset token for user with email {email}.");
                    return null;
                }
                _logger.LogInformation($"Password reset token generated for user with email {email}.");
                return token;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error generating password reset token for user with email {email}: {e.Message}");
                throw new Exception(e.Message);
            }
        }
    #endregion
    }
}