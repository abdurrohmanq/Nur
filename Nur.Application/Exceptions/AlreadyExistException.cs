namespace Nur.Application.Exceptions;

public class AlreadyExistException : Exception
{
    public AlreadyExistException(string message) : base(message)
    {}
    public AlreadyExistException(string message, Exception exception) : base(message, exception)
    {}
    public int StatusCode { get; set; } = 403;
}
