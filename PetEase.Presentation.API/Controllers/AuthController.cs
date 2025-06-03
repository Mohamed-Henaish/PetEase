using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetEase.Core.DTOs;
using PetEase.Services.Interface;

namespace PetEase.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTOs registerDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });

            var result = await _authService.RegisterAsync(registerDTOs);

            if (!result.IsAuthenticated)
                return BadRequest(new { Message = result.Message });

            SetRefrshInCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTOs loginDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDTOs);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefrshInCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefrshInCookies(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeToken revoke)
        {
            var token = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token Is Required");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token Is Invalid!");

            return Ok(new { message = "Revoked Successfully" });
        }

        private void SetRefrshInCookies(string refresh, DateTime expires)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true
            };
            // Pass the cookieOption here
            Response.Cookies.Append("refreshToken", refresh, cookieOption);
        }
    }
}
