using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Authentication
{
    public class JwtService(IOptions<AuthSettings> options)
    {
        // генерация jwt токена
        public string GenerateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim("userName", user.UserName),
                new Claim("id", user.Id.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
