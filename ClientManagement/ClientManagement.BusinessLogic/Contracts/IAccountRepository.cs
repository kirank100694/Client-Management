using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientManagement.BusinessLogic.Contracts
{
    public interface IAccountRepository
    {
        Task<string> Login(SignInModel signInModel);
        Task<IdentityResult> Register(SignUpModel signUpModel);
    }
}