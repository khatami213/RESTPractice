using Core.Contract.Errors;
using System.ComponentModel.DataAnnotations;

namespace Core.DataAnnotations.Base;

public abstract class CustomValidationAttribute : ValidationAttribute
{
    //اگر فرضا چندین پروژه محتلف داشتیم و خواستیم
    //attribute ها برای پروژه های مختلف متفاوت باشند
    //در tempenum پروژه ها را تعریف کرده و به کمک آن میتوانیم
    //مثلا یک فیلد را در یک پروژه اجباری و در پروژه ای دیگر اختیاری قرار داد


    //public List<TempEnum> TempEnums { get; set; } = null;

    //public CustomValidationAttribute(params TempEnum[] tempEnum) => TempEnums = tempEnum.ToList();

    //public CustomValidationAttribute() { }

    protected abstract new Error ErrorMessage { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {

        if (ValidationFilter(value))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new CustomValidationResult(ErrorMessage, new List<string> { validationContext.MemberName });
        }
    }


    public bool ValidationFilter(object value)
    {
        //if (TempEnums is null || TempEnums.Contains(TempConfig.Project))
        //{
        //    return IsValid(value);
        //}
        //else
        //{
        //    return true;
        //}

        return IsValid(value);
    }
}
