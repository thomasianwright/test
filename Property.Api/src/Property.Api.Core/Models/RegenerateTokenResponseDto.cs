namespace Property.Api.Core.Models;

public class RegenerateTokenResponseDto
{
    public string Token { get; set; }
    public DateTime Expiry { get; set; }

    public RegenerateTokenResponseDto(string token, DateTime expiry)
    {
        Token = token;
        Expiry = expiry;
    }
}