using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController(IAuthenticationService authenticationService) : ApiBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct = default)
        {
            var result = await authenticationService.LoginAsync(loginDto, ct); 
            return ToActionResult(result);  
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct = default)
        {
            var result = await authenticationService.RegisterAsync(registerDto, ct); 
            return ToActionResult(result);  
        }


        [HttpGet("emailExists/{email}")]
        public async Task<ActionResult<bool>> CheckEmail(string email, CancellationToken ct = default)
        {
            var result = await authenticationService.CheckEmailExistsAsync(email, ct);
            return ToActionResult(result);
        }


        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

            var result = await authenticationService.GetCurrentUserAsync(email, ct);
            return ToActionResult(result);
        }



        [HttpGet("Adress")]
        [Authorize]
        public async Task<ActionResult<AdressDto>> GetCurrentUserAdress(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

            var result = await authenticationService.GetCurrentUserAdressAsync(email, ct);
            return ToActionResult(result);
        }



        [HttpPut("Adress")]
        [Authorize]
        public async Task<ActionResult<AdressDto>> UpdateCurrentUserAdress(AdressDto dto, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

            var result = await authenticationService.UpdateCurrentUserAdressAsync(email, dto, ct);
            return ToActionResult(result);
        }
    }
}
