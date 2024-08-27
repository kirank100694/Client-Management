using ClientManagement.BusinessLogic.Contracts;
using ClientManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;


        public AuthenticateController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] SignUpModel signUpModel)
        {
            var result = await _accountRepository.Register(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            var result = await _accountRepository.Login(signInModel);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
