using Core.Contract.Infrastructure;

namespace Core.Contract.Application.Events;

public interface IEventRepository : IRepository
{
    void Create(EventModel eventModel);

    void Create(List<EventModel> eventModels);
}
