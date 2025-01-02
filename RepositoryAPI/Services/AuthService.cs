using Microsoft.IdentityModel.Tokens;
using RepositoryAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryAPI.Services
{
    public class AuthService
    {
        private jwtParams _jwtParams { get; set; }

        public AuthService(IConfiguration configuration)
        {
            _jwtParams = new jwtParams { Key = configuration["Jwt:Key"], Audience = configuration["jwt:Audience"], Issuer = configuration["Jwt:Issuer"] };
        }

        public string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtParams.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "admin") 
            };

            var token = new JwtSecurityToken(
                issuer: _jwtParams.Issuer,
                audience: _jwtParams.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
