using Core.Contract.Errors;

namespace Authorization.Application.Exceptions.Resources;

public static class ValidationErrorMessages
{
    public static Error UserIsDuplicated => new Error("1", ResourceValidationErrorMessages.UserIsDuplicated);
    public static Error PasswordAndConfirmPasswordAreNotEqual => new Error("2", ResourceValidationErrorMessages.PasswordAndConfirmPasswordAreNotEqual);
    public static Error InvalidUsernameOrPassword => new Error("3", ResourceValidationErrorMessages.InvalidUsernameOrPassword);
}
