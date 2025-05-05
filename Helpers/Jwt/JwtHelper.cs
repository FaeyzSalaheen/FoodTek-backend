using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Foodtek.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(DataRowCollection collection, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var id = collection[0]["Id"].ToString();
            var IsActive = collection[0]["IsActive"].ToString();
            var role = collection[0]["role"].ToString();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
                new Claim("IsActive", IsActive),
                new Claim("role", role),
                

            };


            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["Jwt:DurationInDays"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
