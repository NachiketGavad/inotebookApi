using inotebookApi.Models;
using inotebookApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace inotebookApi.Helpers.Utils
{
    public class JwtUtils
    {
        private readonly IConfiguration _configuration;
        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Secret key used for signing the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user._id.ToString()),
                    // Add other claims as needed, e.g., user email, name
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            // Read the secret key from configuration
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // Configure token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // Additional validation parameters can be configured as needed
            };

            // Create a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validate and decode the JWT token
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                // Check if the token is a JWT token
                if (!(validatedToken is JwtSecurityToken jwtSecurityToken))
                {
                    throw new SecurityTokenException("Invalid JWT token");
                }

                return claimsPrincipal;
            }
            catch (SecurityTokenException ex)
            {
                // Handle token validation errors
                throw new SecurityTokenException("Token validation failed", ex);
            }
        }
    }
}
