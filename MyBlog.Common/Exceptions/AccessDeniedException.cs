namespace MyBlog.Common.Exceptions;

public class AccessDeniedException(string message) : Exception(message);