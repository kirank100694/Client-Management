using ClientManagement.BusinessLogic.Contracts;
using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClientManagement.BusinessLogic.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }
        public async Task<IdentityResult> Register(SignUpModel signUpModel)
        {
            try
            {
                var user = new IdentityUser()
                {
                    Email = signUpModel.Email,
                    UserName = signUpModel.Email
                };

                return await _userManager.CreateAsync(user, signUpModel.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> Login(SignInModel signInModel)
        {
            string key = "";
            var user = await _userManager.FindByNameAsync(signInModel.UserName);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, signInModel.Password);
                if (result)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                    key = new JwtSecurityTokenHandler().WriteToken(token);
                }
            }

            return key;
        }
    }
}
