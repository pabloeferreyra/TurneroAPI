using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace TurneroAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class LoginAPIController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginAPIController(
        IConfiguration config, SignInManager<IdentityUser> signInManager)
        {
            _config = config;
            _signInManager = signInManager;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            string token = string.Empty;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username or Password null");
            }

            var user = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (!user.Succeeded)
            {
                return BadRequest("Invalid Login and/or password");
            }
            else
            {
                var userLogged = await _signInManager.UserManager.FindByNameAsync(username);
                token = GenerateToken(userLogged);
            }
            var response = "token: " + token;
            return Ok(response);
        }

        private string GenerateToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
            };
            var token = new JwtSecurityToken(_config["Issuer"],
                _config["Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
