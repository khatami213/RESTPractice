using Core.Contract.Application.Commands;
using Core.Contract.Application.Events;
using Core.Contract.Application.Queries;
using Core.Contract.Application.Validations;
using Core.Contract.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Validations;

public class Validator : IValidator
{
    private readonly IEventBus _eventBus;
    private readonly IMapperError _mapperError;

    public Validator(IEventBus eventBus, IMapperError mapperError)
    {
        _eventBus = eventBus;
        _mapperError = mapperError;
    }

    public bool Validate(ICommand command)
    {
        var context = new ValidationContext(command);
        var results = new List<ValidationResult>();
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(command, context, results, true);
        
        var errors = _mapperError.MapList(results).ToList();

        if (!isValid)
            _eventBus.Publish(new ValidationFailed(errors));

        return isValid;
    }

    public bool Validate(IQuery query)
    {
        var context = new ValidationContext(query);
        var results = new List<ValidationResult>();
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(query, context, results, true);

        var errors = _mapperError.MapList(results).ToList();

        if (!isValid)
            _eventBus.Publish(new ValidationFailed(errors));

        return isValid;
    }
}
