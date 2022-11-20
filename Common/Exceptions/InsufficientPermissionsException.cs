namespace Common.Exceptions;

public class InsufficientPermissionsException : System.Exception
{
    public InsufficientPermissionsException(string message) : base(message)
    {
        
    }
}