using Microsoft.IdentityModel.Tokens;
using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Services;

public interface ITokenService
{
    (string token, SecurityToken securityToken) GenerateToken(User user, DateTime expiry);
    string GenerateRefreshToken();
}