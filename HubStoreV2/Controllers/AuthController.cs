using Core.Dto.Auth;
using Core.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HubStoreV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request data", Errors = GetModelErrors() });
            }

            var result = await _authService.RegisterAsync(request);

            if (result.Succeeded)
            {
                return Ok(new 
                { 
                    Succeeded = true,
                    Message = result.Message,
                    User = result.User
                });
            }

            return BadRequest(new { Succeeded = false, Message = result.Message, Errors = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Succeeded = false, Message = "Invalid request data", Errors = GetModelErrors() });
            }

            var result = await _authService.LoginAsync(request);

            if (result.Succeeded)
            {
                return Ok(new 
                { 
                    Succeeded = true,
                    Message = result.Message,
                    User = result.User
                });
            }

            return Unauthorized(new { Succeeded = false, Message = result.Message });
        }

        [HttpPost("login-with-pin")]
        public async Task<IActionResult> LoginWithPin([FromBody] PinLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Succeeded = false, Message = "Invalid request data", Errors = GetModelErrors() });
            }

            var result = await _authService.LoginWithPinCodeAsync(request.PinCode);

            if (result.Succeeded)
            {
                return Ok(new 
                { 
                    Succeeded = true,
                    Message = result.Message,
                    User = result.User
                });
            }

            return Unauthorized(new { Succeeded = false, Message = result.Message });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { Message = "Logged out successfully" });
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync();
            
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }

        private List<string> GetModelErrors()
        {
            var errors = new List<string>();
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors;
        }
    }

}
