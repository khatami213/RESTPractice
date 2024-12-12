using Core.Contract.Errors;

namespace Core.Resources;

public class ValidationErrorMessages
{
    public static Error Required => new Error("1001", ResourceValidationErrorMessages.Required);
    public static Error InValidBirthDate => new Error("1002", ResourceValidationErrorMessages.InValidBirthDate);
}
