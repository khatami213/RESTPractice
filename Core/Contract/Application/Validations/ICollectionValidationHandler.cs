namespace Core.Contract.Application.Validations;

public interface ICollectionValidationHandler
{
}

public interface ICollectionValidationHandler<T> : ICollectionValidationHandler
{
    Task<bool> IsValid(List<T> collection);
}

public interface ICollectionValidationHandler<T, T2> : ICollectionValidationHandler
{
    Task<bool> IsValid(List<T> collection, T2 obj);
}
