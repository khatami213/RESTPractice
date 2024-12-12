using Core.Contract.Application.Commands;
using Core.Contract.Application.Queries;

namespace Core.Contract.Application.Validations;

public interface IValidator
{
    bool Validate(ICommand command);
    bool Validate(IQuery query);
}
