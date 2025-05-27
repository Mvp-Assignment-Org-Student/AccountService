using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;

namespace Business.Services;

public class AccountService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<AccountServiceResult> CreateUser(CreateAccountRequest request)
    {
        var user = new IdentityUser()
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        return result.Succeeded
            ? new AccountServiceResult { Success = true }
            : new AccountServiceResult { Success = false, Error = "Error" };
    }

    public async Task<AccountServiceResult> ExistsByEmail(ExistsRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            return new AccountServiceResult { Success = true, Message = "Email already exists" };
        }

        return new AccountServiceResult { Success = false };
    }

    public async Task<AccountServiceResult> ConfirmEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new AccountServiceResult { Success = false, Error = "User not found" };

        user.EmailConfirmed = true;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? new AccountServiceResult { Success = true }
            : new AccountServiceResult { Success = false, Error = "Could not confirm email" };
    }

}
