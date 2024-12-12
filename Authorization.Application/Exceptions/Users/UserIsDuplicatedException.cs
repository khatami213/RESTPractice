using Authorization.Application.Exceptions.Resources;
using Core.Contract.Application.Exceptions;

namespace Authorization.Application.Exceptions.Users;

public class UserIsDuplicatedException : BusinessException
{
    public UserIsDuplicatedException() : base(ValidationErrorMessages.UserIsDuplicated)
    {
        
    }
}
