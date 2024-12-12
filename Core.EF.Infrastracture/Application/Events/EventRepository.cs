using Core.Contract.Application.Events;
using Core.EF.Infrastracture.EF;

namespace Core.EF.Infrastracture.Application.Events;

public class EventRepository : IEventRepository
{
    private readonly CoreDbContext _coreDbContext;

    public EventRepository(CoreDbContext coreDbContext)
    {
        _coreDbContext = coreDbContext;
    }

    public void Create(EventModel eventModel)
    {
        _coreDbContext.EventModels.Add(eventModel);
    }

    public void Create(List<EventModel> eventModels)
    {
        _coreDbContext.AddRange(eventModels);
    }
}
