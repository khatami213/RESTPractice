using Core.Contract.ServiceLocators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Validations;

public class BaseValidationHandlerFactory
{
    private readonly IServiceLocator _serviceLocator;

    public BaseValidationHandlerFactory(IServiceLocator serviceLocator) => _serviceLocator = serviceLocator;

    public IEnumerable<IBaseValidationHandler<T>> GetAll<T>()
    {
        return _serviceLocator.ResolveAll<IBaseValidationHandler<T>>();
    }

    public IEnumerable<IBaseValidationHandler<T, T2>> GetAll<T, T2>()
    {
        return _serviceLocator.ResolveAll<IBaseValidationHandler<T, T2>>();
    }
}
