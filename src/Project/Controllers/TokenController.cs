using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Project.JWT
{
    [Route("api/v1/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenProviderOptions _options;
        private readonly UserManager<AccountIdentity> _userManager;

        public TokenController(IOptions<TokenProviderOptions> options,
                                UserManager<AccountIdentity> UserManager)
        {
            _options = options.Value;
            _userManager = UserManager;
        }

        private AccountIdentity getUser(string UserName)
        {
            return _userManager.Users.Include(u => u.Recipes).ToList().FirstOrDefault(p => p.UserName == UserName);
        }

        [HttpPost("password")]
        public async Task<IActionResult> SignIn([FromForm]SignInModel signInModel)
        {
            var user = getUser(signInModel.UserName);
            if (await _userManager.CheckPasswordAsync(user, signInModel.Password))
            {
                var now = DateTime.UtcNow;
                var claims = new Claim[]
                {
        new Claim(JwtRegisteredClaimNames.Sub, signInModel.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, RecipeManager.generateUnixTimestamp().ToString(), ClaimValueTypes.Integer64)
                };

                // Create the JWT and write it to a string
                var jwt = new JwtSecurityToken(
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(_options.Expiration),
                    signingCredentials: _options.SigningCredentials);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)_options.Expiration.TotalSeconds
                };
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
            
        }

        public class SignInModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Grant_Type { get; set; }
        } 

    }
}
