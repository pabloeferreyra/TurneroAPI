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

        public LoginAPIController(
        IConfiguration config)
        {
            _config = config;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            string apiUrl = _config.GetValue<string>("base")+"/api/Login";
            WebClient client = new();


            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username or Password null");
            }

            var user = JsonConvert.DeserializeObject<IdentityUser>(client.UploadString(apiUrl + "/Find?username="+ username, string.Empty));
            if (user == null)
            {
                return BadRequest("Invalid Login and/or password");
            }


            //var passwordSignInResult = await LoginController.Access(username, password, isPersistent: true, lockoutOnFailure: false);
            //if (!passwordSignInResult.Succeeded)
            //{
            //    return BadRequest("Invalid Login and/or password");
            //}
            //var token = GenerateToken(user);
            return Ok(user);
            //return Ok("Cookie created");
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
