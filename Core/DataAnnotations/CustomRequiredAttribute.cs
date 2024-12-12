using Core.Contract.Errors;
using Core.DataAnnotations.Base;
using Core.Resources;

namespace Core.DataAnnotations;

public class CustomRequiredAttribute : CustomValidationAttribute
{
    protected override Error ErrorMessage => ValidationErrorMessages.Required;
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        return true;
    }
}
