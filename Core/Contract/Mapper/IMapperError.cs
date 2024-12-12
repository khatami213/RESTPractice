using Core.Contract.Errors;
using System.ComponentModel.DataAnnotations;

namespace Core.Contract.Mapper;

public interface IMapperError
{
    IEnumerable<Error> MapList(List<ValidationResult> source);
}
