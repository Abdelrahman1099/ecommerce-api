using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Class
{
    public class AuthenticationService(IIdentityService identityService, ITokenService tokenService) : IAuthenticationService
    {
        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
        {
            var result = await identityService.FindUserByEmailAsync(email, ct);
            if (result.IsSuccess)
                return Result<bool>.Ok(true);
            return Result<bool>.Fail(result.Errors);
        }

        public async Task<Result<AdressDto>> GetCurrentUserAdressAsync(string email, CancellationToken ct = default)
        {
            return await identityService.GetCurrentUserAdressAsync(email, ct);
        }

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var userResult = await identityService.FindUserByEmailAsync(email, ct);
            if(userResult.IsSuccess)
            {
                var user = userResult.Data;
                var roles = await identityService.GetUserRolesAsync(user.Email);
                var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.DisplayName, roles.Data);

                return Result<UserDto>.Ok(new UserDto()
                {
                    DisplayName = userResult.Data.DisplayName,
                    Email = userResult.Data.Email,
                    Token = token

                });
            }
            else
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var UserResult = await identityService.FindUserByEmailAsync(loginDto.Email);

            if (!UserResult.IsSuccess)
                return Result<UserDto>.Fail(UserResult.Errors);

            var PasswordResult = await identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);

            if (!PasswordResult.IsSuccess)
                return Result<UserDto>.Fail(PasswordResult.Errors);

            var rolesResult = await identityService.GetUserRolesAsync(UserResult.Data.Email, ct);

            var token = await tokenService.CreateTokenAsync(UserResult.Data.Id, UserResult.Data.Email, UserResult.Data.UserName, rolesResult.Data);

            return Result<UserDto>.Ok(new UserDto()
            {
                Email = UserResult.Data.Email,
                Token = token,
                DisplayName = UserResult.Data.DisplayName
            });
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var UserResult = await identityService.CreateUserAsync(registerDto, ct);

            if (!UserResult.IsSuccess)
                return Result<UserDto>.Fail(UserResult.Errors);

            var rolesResult = await identityService.GetUserRolesAsync(UserResult.Data.Email, ct);

            var token = await tokenService.CreateTokenAsync(UserResult.Data.Id, UserResult.Data.Email, UserResult.Data.UserName, rolesResult.Data);


            return Result<UserDto>.Ok(new UserDto()
            {
                DisplayName = UserResult.Data.DisplayName,
                Email = UserResult.Data.Email,
                Token = token
            });
        }

        public async Task<Result<AdressDto>> UpdateCurrentUserAdressAsync(string email, AdressDto dto, CancellationToken ct = default)
        {
            return await identityService.UpdateCurrentUserAdressAsync(email, dto, ct);
        }
    }
}
