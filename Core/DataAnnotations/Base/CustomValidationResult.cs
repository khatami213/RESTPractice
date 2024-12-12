using Core.Contract.Errors;
using System.ComponentModel.DataAnnotations;

namespace Core.DataAnnotations.Base;

public class CustomValidationResult : ValidationResult
{
    public string ErrorCode { get; private set; }

    public List<CustomValidationResult>? InnerValidationResult { get; set; }

    public CustomValidationResult(Error errorMessage, IEnumerable<string> memberNames) : base(errorMessage.Message, memberNames)
    {
        ErrorCode = errorMessage.Code;

        InnerValidationResult = errorMessage.Errors?.Select(x => new CustomValidationResult(x, new List<string> { x.FieldName }))?.ToList();
    }
}
