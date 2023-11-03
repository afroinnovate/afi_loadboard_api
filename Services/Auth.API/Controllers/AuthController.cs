using Auth.API.Dtos;
using Auth.API.Services;
using Auth.API.Services.IServices;
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


        [HttpPost]
        [Route("api/auth/addroles")]
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
    }
}