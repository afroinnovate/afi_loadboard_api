using Auth.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Health()
        {
            return Ok("Auth API is running");
        }

        [HttpPost]
        [Route("api/auth/login")]
        public IActionResult login([FromBody] LoginRequestDto request)
        {
            ResponseDto response = new()
            {
                IsSuccess = true,
                Message = "You've logged in.",
                Result = request
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("api/auth/register")]
        public IActionResult register([FromBody] RegistrationRequestDto request)
        {
            ResponseDto response = new()
            {
                IsSuccess = true,
                Message = "You've registered succef.",
                Result = request
            };
            return Ok(response);
        }


        [HttpPost]
        [Route("api/auth/addroles")]
        public IActionResult addRole([FromBody] RoleRequestDto request)
        {
            ResponseDto response = new()
            {
                IsSuccess = true,
                Message = $"You've successfully added the role {request.RoleName} for {request.Email}.",
                Result = request
            };
            return Ok(response);
        }
    }
}