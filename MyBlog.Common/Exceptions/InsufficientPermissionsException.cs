namespace MyBlog.Common.Exceptions;

public class InsufficientPermissionsException : Exception
{
    public InsufficientPermissionsException(string message) : base(message)
    {
    }
}