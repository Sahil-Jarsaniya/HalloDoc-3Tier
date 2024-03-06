using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class JwtService: IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(LoggedUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("AspId", user.AspId),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(20);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials:creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string  token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if(token ==  null)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey= true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                if(jwtSecurityToken != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
