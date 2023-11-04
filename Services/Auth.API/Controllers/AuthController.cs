using Auth.API.Dtos;
using Auth.API.Services;
using Auth.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authServices;
        private readonly ResponseDto _response;
        public AuthController(ILogger<AuthController> logger, IAuthService authServices)
        {
            _logger = logger;
            _authServices = authServices;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Route("/health")]
        public IActionResult Health()
        {
            return Ok("Auth API is running");
        }

        [HttpPost]
        [Route("api/auth/login")]
        public IActionResult login([FromBody] LoginRequestDto request)
        {
           
                // Check ModelState validity first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (request.Email == null && request.UserName == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please provide either email or username";
                    _response.Result = null;
                    return BadRequest(_response);
                }

                var loginType = string.IsNullOrEmpty(request.Email) ? "username" : "email";

                var loginResponse = _authServices.Login(request, loginType).Result;

                if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
                {

                    _response.IsSuccess = false;
                    _response.Message = "Email or password is wrong.";
                    _response.Result = null;
                    return Unauthorized(_response);
                }
                _response.IsSuccess = true;
                _response.Message = "You've logged in sucessfully.";
                _response.Result = loginResponse;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {nameof(login)}");
                return StatusCode(500, $"Internal server error {e}");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                var response = _authServices.logout();
                _logger.LogInformation($"User {response} logged out successfully.");
                return Ok("Logged out successfully.");
                
            }
            catch (Exception e)
            {
                // ... Log the error
                _logger.LogError(e, "An error occurred while logging out.");
                return StatusCode(500, "An error occurred while logging out.");
            }
        }

        [HttpPost]
        [Route("api/auth/register/{rolenames}")]
        public IActionResult register([FromBody] RegistrationRequestDto request, string? rolenames)
        {
            // Check ModelState validity first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (request.Email == null && request.UserName.ToLower() == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Either Email or Username must be provided";
                    _response.Result = null;
                    return BadRequest(_response);
                }

                var registrationResponse = _authServices.Register(request, rolenames.ToUpper()).Result;
                if (!registrationResponse.IsSuccess)
                {
                    _logger.LogError(registrationResponse.Message);
                    _response.IsSuccess = false;
                    _response.Message = "Somthing was wrong with your registration";
                    _response.Result = registrationResponse;
                    return BadRequest(_response);
                }
                _response.Message = "You've registered successfully.";
                _response.Result = request;
                _response.IsSuccess = true;
                _logger.LogError("You've registered successfully.");
                return Ok(_response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {nameof(register)}");
                return StatusCode(500, $"Internal server error {e}");
            }
        }

        [Authorize(Roles = "ADMIN, CUS_ADMIN")]
        [HttpPost]
        [Route("api/auth/add-roles")]
        public IActionResult addRole([FromBody] RoleRequestDto request)
        {
            // Check ModelState validity first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var roleResponse = _authServices.AssignRole(request.Email, request.RoleName.ToUpper()).Result;

                if (!roleResponse)
                {
                    _logger.LogError("Something was wrong with role assignment");
                    _response.IsSuccess = false;
                    _response.Message = "Somthing was wrong with role assignment";
                    _response.Result = null;
                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.Message = $"You've successfully added the role {request.RoleName} for {request.Email}.";
                _logger.LogInformation($"You've successfully added the role {request.RoleName} for {request.Email}.");
                return Ok(_response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "ADMIN, CUS_ADMIN")]
        [HttpDelete]
        [Route("api/auth/remove-roles")]
        public IActionResult RemoveRole([FromBody] RoleRequestDto request)
        {
            // Check ModelState validity first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var roleResponse = _authServices.RemoveRole(request.Email, request.RoleName.ToUpper()).Result;

                if (!roleResponse)
                {
                    _logger.LogError("Something was wrong with role Removal");
                    _response.IsSuccess = false;
                    _response.Message = "Somthing was wrong with role Removal";
                    _response.Result = null;
                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.Message = $"You've successfully remove the role {request.RoleName} from {request.Email}.";
                _logger.LogInformation($"You've successfully removed the role {request.RoleName.ToUpper()} from user {request.Email}.");
                return Ok(_response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("api/auth/reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            // Check ModelState validity first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resetPasswordResponse = _authServices.ResetPasswordAsync(request.Email, request.Token, request.NewPassword).Result;

                if (!resetPasswordResponse.IsSuccess)
                {
                    _logger.LogError("Something was wrong with password reset");
                    _response.IsSuccess = false;
                    _response.Message = "Somthing was wrong with password reset";
                    _response.Result = null;
                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.Message = $"You've successfully reset your password.";
                _logger.LogInformation($"You've successfully reset your password.");
                return Ok(_response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("api/auth/{email}")]
        public IActionResult GetResetPasswordToken(string email)
        {
            // Check ModelState validity first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resetPasswordResponse = _authServices.GeneratePasswordResetTokenAsync(email).Result;

                if (!string.IsNullOrEmpty(resetPasswordResponse))
                {
                    _logger.LogError("Something was wrong with password reset");
                    _response.IsSuccess = false;
                    _response.Message = "Somthing was wrong with password reset";
                    _response.Result = null;
                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.Message = $"You've successfully reset your password.";
                _response.Result = resetPasswordResponse;
                _logger.LogInformation($"You've successfully reset your password.");
                return Ok(_response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}