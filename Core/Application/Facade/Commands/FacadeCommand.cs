using Core.Contract.Application.Events;
using Core.Contract.Errors;
using Core.Contract.Facade;
using Core.Contract.Response;

namespace Core.Application.Facade.Commands;

public abstract class FacadeCommand : IFacadeCommand
{
    protected readonly IEventBus _eventBus;
    private IEnumerable<IError> _errors;
    private bool _status;

    protected FacadeCommand(IEventBus eventBus)
    {
        _eventBus = eventBus;
        _status = true;

        _eventBus.Subscribe<ValidationFailed>(v =>
        {
            _status = false;
            _errors = v.Errors;
        });

        _eventBus.Subscribe<CollectionValidationsFailed>(v =>
        {
            _status = false;
            _errors = v.ValidationErrors;
        });
    }

    protected Result Return(object data = null)
    {
        return new Result(_status, _errors, data);
    }
}
