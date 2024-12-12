using Core.Contract.Application.Events;
using Core.Contract.Errors;
using Core.Contract.Facade;
using Core.Contract.Response;

namespace Core.Application.Facade.Queries;

public class FacadeQuery : IFacadeQuery
{
    private bool _status;
    protected FacadeQuery()
    {
        _status = true;
    }

    protected Result Return(object data = null)
    {
        return new Result(_status, null, data);
    }
}
