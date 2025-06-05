using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace Business.Services;

public class AccountService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, JwtService jwtService) : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly JwtService _jwtService = jwtService; 


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
    public async Task<AccountServiceResult> LogInUser(LoginUserRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
        if (!result.Succeeded)
        {
            return new AccountServiceResult { Success = false, Error = "Login failed" };
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        var token = _jwtService.GenerateToken(user!);

        return new AccountServiceResult
        {
            Success = true,
            Token = token
        };
    }

    public async Task<AccountServiceResult> LogOutUser()
    {
        await _signInManager.SignOutAsync();


        return new AccountServiceResult { Success = true };
    }

    public async Task<AccountServiceResult> ExistsByEmail(ExistsRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            return new AccountServiceResult { Success = true, Message = "Email already exists" };
        }

        return new AccountServiceResult { Success = false, Message = "Email doesn't exists" };
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
