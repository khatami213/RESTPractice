using Core.Contract.Application.Events;
using Core.Contract.Application.Validations;
using Core.Contract.Errors;
using Core.ExtentionMethods.Base;
using System.Diagnostics;

namespace Core.Application.Validations;

public class CollectionValidationHandler<T> : ICollectionValidationHandler<T> 
{
    private readonly IEventBus _eventBus;
    private readonly BaseValidationHandlerFactory _baseValidationHandlerFactory;
    private readonly List<ValidationCollectionError> _validationCollectionErrors;

    public CollectionValidationHandler(IEventBus eventBus, BaseValidationHandlerFactory baseValidationHandlerFactory, List<ValidationCollectionError> validationCollectionErrors)
    {
        _eventBus = eventBus;
        _baseValidationHandlerFactory = baseValidationHandlerFactory;
        _validationCollectionErrors = validationCollectionErrors;
    }

    public async Task<bool> IsValid(List<T> collection)
    {
        collection.ForEach((rowNumber, x) =>
        {
            _validationCollectionErrors.Add(new ValidationCollectionError { RowId = rowNumber });
        });

        var mth = new StackTrace().GetFrame(6).GetMethod();
        var cls = mth.ReflectedType.Namespace.Remove(0, mth.ReflectedType.Namespace.LastIndexOf("."));

        var validationHandlers = _baseValidationHandlerFactory.GetAll<T>();

        foreach (var validationHandler in validationHandlers)
        {
            await validationHandler.Validate(collection);
        }

        if (_validationCollectionErrors.Where(x => x.Errors.Count > 0).Any())
        {
            _eventBus.Publish(new CollectionValidationsFailed
            {
                ValidationErrors = _validationCollectionErrors.Where(x => x.Errors.Count > 0).ToList()
            });

            return false;
        }

        return true;
    }

    protected void AddError(int rowId, Error error)
    {
        var validationCollectionError = _validationCollectionErrors.FirstOrDefault(x => x.RowId == rowId);

        validationCollectionError.Errors.Add(error);
    }

    protected void AddError(ValidationCollectionError validation, Error error)
    {
        validation.Errors.Add(error);
    }

    protected List<ValidationCollectionError> GetErrors()
    {
        return _validationCollectionErrors.Where(x => x.Errors.Count > 0).ToList();
    }

    protected ValidationCollectionError GetError(int rowId)
    {
        return _validationCollectionErrors.FirstOrDefault(x => x.RowId == rowId);
    }
}

public class CollectionValidationHandler<T, T2> : ICollectionValidationHandler<T, T2> 
{
    private readonly IEventBus _eventBus;
    private readonly BaseValidationHandlerFactory _baseValidationHandlerFactory;
    private readonly List<ValidationCollectionError> _validationCollectionErrors;

    public CollectionValidationHandler(IEventBus eventBus, BaseValidationHandlerFactory baseValidationHandlerFactory, List<ValidationCollectionError> validationCollectionErrors)
    {
        _eventBus = eventBus;
        _baseValidationHandlerFactory = baseValidationHandlerFactory;
        _validationCollectionErrors = validationCollectionErrors;
    }

    public async Task<bool> IsValid(List<T> collection, T2 obj)
    {
        collection.ForEach((rowNumber, x) =>
        {
            _validationCollectionErrors.Add(new ValidationCollectionError { RowId = rowNumber });
        });

        var validationHandlers = _baseValidationHandlerFactory.GetAll<T, T2>();

        foreach (var validationHandler in validationHandlers)
        {
            await validationHandler.Validate(collection, obj);
        }

        if (_validationCollectionErrors.Where(x => x.Errors.Count > 0).Any())
        {
            _eventBus.Publish(new CollectionValidationsFailed
            {
                ValidationErrors = _validationCollectionErrors.Where(x => x.Errors.Count > 0).ToList()
            });

            return false;
        }

        return true;
    }

    protected void AddError(int rowId, Error error)
    {
        var validationCollectionError = _validationCollectionErrors.FirstOrDefault(x => x.RowId == rowId);

        validationCollectionError.Errors.Add(error);
    }

    protected void AddError(ValidationCollectionError validation, Error error)
    {
        validation.Errors.Add(error);
    }

    protected List<ValidationCollectionError> GetErrors()
    {
        return _validationCollectionErrors.Where(x => x.Errors.Count > 0).ToList();
    }

    protected ValidationCollectionError GetError(int rowId)
    {
        return _validationCollectionErrors.FirstOrDefault(x => x.RowId == rowId);
    }
}

public interface IBaseValidationHandler<T>
{
    Task Validate(List<T> collection);
}

public abstract class BaseValidationHandler<T> : IBaseValidationHandler<T>
{
    private readonly List<ValidationCollectionError> _validationCollectionErrors;

    public BaseValidationHandler(List<ValidationCollectionError> validationCollectionErrors) => _validationCollectionErrors = validationCollectionErrors;

    public abstract Task Validate(List<T> collection);

    protected void AddError(int rowId, Error error)
    {
        var validationCollectionError = _validationCollectionErrors.FirstOrDefault(x => x.RowId == rowId);

        validationCollectionError.Errors.Add(error);
    }
}

public interface IBaseValidationHandler<T, T2>
{
    Task Validate(List<T> collection, T2 obj);
}

public abstract class BaseValidationHandler<T, T2> : IBaseValidationHandler<T, T2>
{
    private readonly List<ValidationCollectionError> _validationCollectionErrors;

    public abstract Task Validate(List<T> collection, T2 obj);

    public BaseValidationHandler(List<ValidationCollectionError> validationCollectionErrors) => _validationCollectionErrors = validationCollectionErrors;

    public void AddError(int rowNumber, Error error)
    {
        var validationCollectionError = _validationCollectionErrors.FirstOrDefault(x => x.RowId == rowNumber);

        validationCollectionError.Errors.Add(error);
    }
}
