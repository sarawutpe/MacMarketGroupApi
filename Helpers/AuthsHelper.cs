using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace MacMarketGroupApi.Services;

public class IJWT
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class IJWTVerify
{
    public bool IsVerify { get; set; }
    public string Message { get; set; }
}

public class AuthsHelper
{
    private readonly IConfiguration _configuration;
    public AuthsHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IJWT GenerateToken(string userId)
    {
        try
        {
            var secretKey = _configuration?["JwtSettings:SecretKey"] ?? "";
            var issuer = _configuration?["JwtSettings:Issuer"] ?? "";
            var audience = _configuration?["JwtSettings:Audience"] ?? "";
            var expirationInHours = _configuration?["JwtSettings:ExpirationInHours"] ?? "";
            var refreshExpirationInHours = _configuration?["JwtSettings:RefreshExpirationInHours"] ?? "";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Add additional claims as needed
            var claims = new[]
            {
            new Claim("id", userId),
        };

            var now = DateTime.UtcNow;

            // Defined Jwt token payload
            var tokenPayload = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: now.AddHours(Convert.ToDouble(expirationInHours)),
                signingCredentials: credentials
            );

            // Defined Jwt refresh token payload
            var refreshTokenPayload = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: now.AddHours(Convert.ToDouble(refreshExpirationInHours)),
                signingCredentials: credentials
             );

            var newToken = new JwtSecurityTokenHandler().WriteToken(tokenPayload);
            var newRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshTokenPayload);

            return new IJWT
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
            };
        }
        catch
        {
            return new IJWT
            {
                Token = "",
                RefreshToken = "",
            };
        }
    }

    public IJWTVerify VerifyJwtToken(string token)
    {
        try
        {
            IdentityModelEventSource.ShowPII = true;

            var secretKey = _configuration?["JwtSettings:SecretKey"] ?? "";
            var issuer = _configuration?["JwtSettings:Issuer"] ?? "";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                IssuerSigningKey = securityKey
            };

            // Validate token and extract claims
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);

            return new IJWTVerify
            {
                IsVerify = true,
                Message = "Token verification successful."
            };
        }
        catch (SecurityTokenExpiredException)
        {
            return new IJWTVerify
            {
                IsVerify = false,
                Message = "Token has expired."
            };
        }
        catch (Exception error)
        {
            return new IJWTVerify
            {
                IsVerify = false,
                Message = error.Message
            };
        }
    }

}