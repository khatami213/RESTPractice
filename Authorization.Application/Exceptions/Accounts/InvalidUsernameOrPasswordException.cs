using Authorization.Application.Exceptions.Resources;
using Core.Contract.Application.Exceptions;

namespace Authorization.Application.Exceptions.Accounts;

public class InvalidUsernameOrPasswordException : BusinessException
{
    public InvalidUsernameOrPasswordException() : base(ValidationErrorMessages.InvalidUsernameOrPassword)
    {
        
    }
}
