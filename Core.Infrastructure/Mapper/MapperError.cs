using Core.Contract.Errors;
using Core.Contract.Mapper;
using Core.DataAnnotations.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Mapper;

public class MapperError : IMapperError
{
    public IEnumerable<Error> MapList(List<ValidationResult> source)
    {
        var result = source.Select(x =>
        {
            var customValidationResult = (CustomValidationResult)x;
            var error = new Error(customValidationResult.ErrorCode,
                  x.ErrorMessage,
                  x.MemberNames.FirstOrDefault());

            if (customValidationResult.InnerValidationResult is not null)
            {
                InnerMap(customValidationResult, error);
            }

            return error;
        });


        return result;
    }

    private static void InnerMap(CustomValidationResult customValidationResult, Error error)
    {
        error.Errors = customValidationResult.InnerValidationResult
        .Select(x =>
        {
            var error = new Error(x.ErrorCode,
                 x.ErrorMessage,
                 x.MemberNames.FirstOrDefault());

            if (x.InnerValidationResult is not null)
            {
                InnerMap(x, error);
            }

            return error;
        }
        ).ToList();

    }
}
