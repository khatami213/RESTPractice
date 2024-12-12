using Authorization.Application.Exceptions.Resources;
using Core.Contract.Application.Exceptions;

namespace Authorization.Application.Exceptions.Users;

public class PasswordAndConfirmPasswordAreNotEqualException : BusinessException
{
    public PasswordAndConfirmPasswordAreNotEqualException() : base(ValidationErrorMessages.PasswordAndConfirmPasswordAreNotEqual)
    {
        
    }
}
