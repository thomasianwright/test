namespace Property.Api.Core.Models;

public class AuthenticateResponseDto
{
    public string Token { get; set; }
    public DateTime TokenExpiry { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public string UserId { get; set; }
    public UserDto User { get; set; }
    public AuthenticateResponseDto(string token, DateTime tokenExpiry, string refreshToken, DateTime refreshTokenExpiry, string userId, UserDto user)
    {
        Token = token;
        TokenExpiry = tokenExpiry;
        RefreshToken = refreshToken;
        RefreshTokenExpiry = refreshTokenExpiry;
        UserId = userId;
        User = user;
    }

    public AuthenticateResponseDto()
    {
        
    }
}