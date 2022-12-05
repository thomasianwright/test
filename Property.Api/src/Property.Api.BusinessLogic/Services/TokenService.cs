using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HashidsNet;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Property.Api.BusinessLogic.Options;
using Property.Api.Contracts.Services;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.Services;

public class TokenService : ITokenService
{
    private readonly IHashids _hashids;
    private readonly JwtOptions _jwtOptions;

    public TokenService(IOptions<JwtOptions> jwtOptions, IHashids hashids)
    {
        _hashids = hashids;
        _jwtOptions = jwtOptions.Value;
    }

    public (string token, SecurityToken securityToken) GenerateToken(User user, DateTime expiry)
    {
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);
        var userId = _hashids.Encode(user.Id);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            }),
            Expires = expiry,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return (jwtToken, token);
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}