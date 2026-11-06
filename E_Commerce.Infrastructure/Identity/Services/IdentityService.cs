using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
        public async Task<Result<bool>> CheckPasswordAsync(string Email, string Password, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user is null)
                return Result<bool>.Fail(Error.InvalidCredentials("User.InvalidCredentials", "Email or Passord Invalid"));

            var result = await userManager.CheckPasswordAsync(user, Password);
            return result ? Result<bool>.Ok(result) : Result<bool>.Fail(Error.InvalidCredentials("User.InvalidCredentials"));
        }

        public async Task<Result<AdressDto>> GetCurrentUserAdressAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(U => U.adress).Where(U => U.Email == email).FirstOrDefaultAsync(ct);
            if (user is null)
                return Result<AdressDto>.Fail(Error.NotFound("User.NotFound", $"User with Email {email}, Not Found"));

            if (user.adress is null)
                return Result<AdressDto>.Fail(Error.NotFound("User.NotFound", $"User with Email {email}, does Not have Adress"));

            return Result<AdressDto>.Ok(new AdressDto() 
            {
                Street = user.adress.Street,
                City = user.adress.City,
                Country = user.adress.Country,
                FirstName = user.adress.FirstName,
                LastName = user.adress.LastName,
            });
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
            };

            var CreateResult = await userManager.CreateAsync(user, registerDto.Password);
            if(!CreateResult.Succeeded)
            {
                var errors = CreateResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string Email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if(user is null)

                return Result<IdentityUserResult>.Fail(Error.NotFound("User.NotFound", $"User with Email {Email} not found"));
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound("User.NotFound", $"User with Email {email} not found"));

            var roles = await userManager.GetRolesAsync(user);

            return Result<IReadOnlyList<string>>.Ok(roles.ToList());
        }

        public async Task<Result<AdressDto>> UpdateCurrentUserAdressAsync(string email, AdressDto dto, CancellationToken ct = default)
        {
            ApplicationUser applicationUser = await userManager.Users.Include(U => U.adress).Where(U => U.Email == email).FirstOrDefaultAsync(ct);
            var user = applicationUser;
            if (user is null)
                return Result<AdressDto>.Fail(Error.NotFound("User.NotFound", $"User with Email {email}, Not Found"));

            if (user.adress is null)
            {
                user.adress = new Adress()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Street = dto.Street,
                    City = dto.City,
                    Country = dto.Country,

                };
            }
            else
            {
                user.adress.FirstName = dto.FirstName;
                user.adress.LastName = dto.LastName;
                user.adress.Street = dto.Street;
                user.adress.City = dto.City;
                user.adress.Country = dto.Country;     

            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<AdressDto>.Fail(Error.Failure("Failure", "Cannot Update Or Create Address"));

            return Result<AdressDto>.Ok(dto);
        }
    }
}
