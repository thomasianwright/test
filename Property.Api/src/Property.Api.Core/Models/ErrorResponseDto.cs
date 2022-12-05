namespace Property.Api.Core.Models;

public class ErrorResponseDto
{
    public string Message { get; set; }
    public int? ErrorCode { get; set; }

    public ErrorResponseDto(string message, int? errorCode)
    {
        Message = message;
        ErrorCode = errorCode;
    }
}