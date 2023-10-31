using Auth.API.Dtos;
using Auth.API.Models;
using Auth.API.Repository;
using Auth.API.Services.IServices;
using AutoMapper;

namespace Auth.API.Services
{
    class AuthService : IAuthService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, 
        ILogger<AuthService> logger, IMapper mapper)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> AssignRole(string userId, string roleName)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogError($"User with ID {userId} not found.");
                    return false;
                }

                var result = await _userRepository.AssignRole(userId, roleName);
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
                return false;
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                _logger.LogError("Login request data transfer object is null.");
                return new LoginResponseDto();
            }

            try
            {
                //Login user
                var result = await _userRepository.Login(loginRequestDto);
                if (result == null)
                {
                    _logger.LogError($"Error logging in user with username {loginRequestDto.UserName}.");
                    return new LoginResponseDto();
                }
                else 
                {
                    //Get the user's role
                    var roles = await _userRepository.GetUserRole(result.User.Email);
                    
                    if (roles.Count == 0)
                    {
                        _logger.LogError($"Error retrieving role for user with username {loginRequestDto.UserName}.");
                        return new LoginResponseDto();
                    }

                    //map userDto to applicationUser
                    var user = _mapper.Map<ApplicationUser>(result.User);

                    //Generate token
                    var token = _jwtTokenGenerator.GenerateToken(user, roles);
                    if (string.IsNullOrEmpty(token))
                    {
                        _logger.LogError($"Error generating token for user with username {loginRequestDto.UserName}.");
                        return new LoginResponseDto();
                    }

                    //Return login response
                    UserDto userDto = new()
                    {
                        Email = result.User.Email,
                        FirstName = result.User.FirstName,
                        LastName = result.User.LastName,
                        MiddleName = result.User.MiddleName,
                        PhoneNumber = result.User.PhoneNumber,
                        UserName = result.User.UserName
                    };

                    _logger.LogInformation($"User with username {loginRequestDto.UserName} logged in successfully.");
                    return new LoginResponseDto
                    {
                        Token = token,
                        User = userDto
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error logging in user with username {loginRequestDto.UserName}: {e.Message}");
                return new LoginResponseDto();
            }
        }

        public async Task<string> Register(RegistrationRequestDto registerationRequestDto, string roleName)
        {
            if (registerationRequestDto == null || registerationRequestDto.Password == null)
            {
                _logger.LogError("Registration request data or transfer object is null or password.");
                return "Error registering user";
            }
            try
            {
                var appUser = _mapper.Map<ApplicationUser>(registerationRequestDto);
                var response = await _userRepository.Register(appUser, registerationRequestDto.Password, roleName);

                const string successMessage = "User with username {0} registered successfully.";
                const string errorMessage = "Error registering user with username {0}.";

                if (response.Contains("Registration successful"))
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
                return "Error registering user";
            }
        }
    }
}