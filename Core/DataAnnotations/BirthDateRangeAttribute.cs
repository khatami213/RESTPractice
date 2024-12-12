using Core.Contract.Errors;
using Core.DataAnnotations.Base;
using Core.Resources;

namespace Core.DataAnnotations;

public class BirthDateRangeAttribute : CustomValidationAttribute
{
    protected override Error ErrorMessage => ValidationErrorMessages.InValidBirthDate;
    private DateTime Minimum {  get; set; }
    private DateTime Maximum { get; set; }

    public BirthDateRangeAttribute()
    {
        Minimum = new DateTime(1900, 01, 01);
        Maximum = DateTime.Now;
    }

    public override bool IsValid(object? value)
    {
        if (value != null) 
        {
            var dateTime = (DateTime)value;

            if (dateTime < Minimum || dateTime > Maximum)
                return false;
        }

        return true;

    }
}
