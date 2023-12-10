using System.Net;
using CODL.Models;

namespace CODL.Exceptions;

public class ServiceException : Exception
{
    private HttpStatusCode _statusCode;
    private ServiceResponse _response;

    public ServiceException(string message, HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        _response = new ServiceResponse
        {
            Success = false,
            Message = message
        };
    }

    public HttpStatusCode StatusCode
    {
        get => _statusCode;
    }

    public ServiceResponse Response
    {
        get => _response;
    }
}

public class ResourceNotFoundException : ServiceException
{
    public ResourceNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}

public class ResourceAlreadyExistsException : ServiceException
{
    public ResourceAlreadyExistsException(string message) : base(message, HttpStatusCode.Conflict)
    {
    }
}

public class BadRequestException : ServiceException
{
    public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}

public class InvalidCredentialsException : ServiceException
{
    public InvalidCredentialsException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}