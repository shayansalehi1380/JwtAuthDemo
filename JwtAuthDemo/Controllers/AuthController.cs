using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthDemo.Models;
using JwtAuthDemo.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user.UserName == "Ali" && user.Password == "1234")
            {
                var accessToken = GenerateJwtToken(user.UserName);
                var refreshToken = GenerateRefreshToken();

                RefreshTokenStore.Tokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    UserName = user.UserName,
                    ExpiryDate = DateTime.Now.AddDays(7)
                });

                return Ok(new
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refreshToken)
        {
            var existingToken = RefreshTokenStore.Tokens.FirstOrDefault(t => t.Token == refreshToken);

            if (existingToken == null || existingToken.ExpiryDate < DateTime.Now)
            {
                return Unauthorized("Refresh token is invalid or expired.");
            }

            var newAccessToken = GenerateJwtToken(existingToken.UserName);

            return Ok(new
            {
                access_token = newAccessToken
            });
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey1234567890LongEnoughForJwt"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "myapi",
                audience: "myapi",
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        }
    }
}

