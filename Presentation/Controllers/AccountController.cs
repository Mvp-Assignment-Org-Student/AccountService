using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateAccountRequest request)
    {
        var result = await _accountService.CreateUser(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var result = await _accountService.LogInUser(request);

        return result.Success ? Ok(result) : BadRequest(result);

    }

    [HttpPost("exists/email")]
    public async Task<IActionResult> EmailExists(ExistsRequest request)
    {
        var result = await _accountService.ExistsByEmail(request);

        return result.Success ? BadRequest() : Ok(result);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await _accountService.ConfirmEmailAsync(request.Email);
        return result.Success ? Ok() : BadRequest(result.Error);
    }

}
