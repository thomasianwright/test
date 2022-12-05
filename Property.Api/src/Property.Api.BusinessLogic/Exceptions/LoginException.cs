namespace Property.Api.BusinessLogic.Exceptions;

public class LoginException : Exception
{
    public int Code { get; init; }
    
    public LoginException(string message, int code) : base(message)
    {
        Code = code;
    }
}