namespace EmailMarketingWebApi.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public interface ITokenGenerator
    {
        string GenerateToken(string userId);
    }

    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
            // Add other claims as needed
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"], // Set in appsettings.json
                audience: _configuration["JwtSettings:Audience"], // Set in appsettings.json
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
