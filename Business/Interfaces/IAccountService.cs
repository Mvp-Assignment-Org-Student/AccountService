using Business.Models;

namespace Business.Interfaces;

public interface IAccountService
{
    Task<AccountServiceResult> ConfirmEmailAsync(string email);
    Task<AccountServiceResult> CreateUser(CreateAccountRequest request);
    Task<AccountServiceResult> ExistsByEmail(ExistsRequest request);
    Task<AccountServiceResult> LogInUser(LoginUserRequest request);
    Task<AccountServiceResult> LogOutUser();
}