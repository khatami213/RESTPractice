using Core.Contract.Errors;

namespace Core.Contract.Application.Exceptions;

public class BusinessException : Exception
{
    public string ErrorCode { get; protected set; }

    public string ErrorMessage { get; protected set; }

    public BusinessException()
    {

    }

    public BusinessException(Error errorMessage)
    {
        ErrorCode = errorMessage.Code;
        ErrorMessage = errorMessage.Message;
    }
}
