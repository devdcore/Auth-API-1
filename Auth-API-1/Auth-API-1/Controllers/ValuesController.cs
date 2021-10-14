using Auth_API_1.Areas.Identity.Data;
using Auth_API_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth_API_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {

        private Auth_API_1Context _dbContext;
        private UserManager<Auth_API_1User> _userManager;
        private SignInManager<Auth_API_1User> _signInManager;

        public ValuesController(Auth_API_1Context dbContext,
                                UserManager<Auth_API_1User> userManager,
                                SignInManager<Auth_API_1User> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("getFruits")]
        [AllowAnonymous]
        public ActionResult GetFruits()
        {
            List<string> mylist = new List<string>() { "apple", "bannanas" };

            return Ok(mylist);
        }


        [HttpGet("getFruitsAuthenticated")]
        public ActionResult GetFruitsAuthenticated()
        {
            List<string> mylist = new List<string>() { "organic apple", "organic bannanas" };

            return Ok(mylist);
        }

        [AllowAnonymous]
        [HttpPost("getToken")]
        public async Task<ActionResult> GetToken([FromBody] LoginModel loginModel)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginModel.Email);
            if(user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

                if(signInResult.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("MY_BIG_SECRET_KEY_LKSHDJFLSDKJFW@#($)(#)34234");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, loginModel.Email)
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Ok("Failed, try again");
                }
            }
            return Ok("Failed, try again");
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] LoginModel loginModel)
        {
            Auth_API_1User auth_API_1User = new Auth_API_1User()
            {
                Email = loginModel.Email,
                UserName = loginModel.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(auth_API_1User, loginModel.Password);

            if(result.Succeeded)
            {
                return Ok(new { Result = "Register Success" });
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach(var error in result.Errors)
                {
                    stringBuilder.Append(error.Description);
                    stringBuilder.Append("ERROR");
                }
                return Ok(new { Result = $"Register Fail: {stringBuilder.ToString()}" });
            }
        }

   }
}
