using Core.Contract.Application.Commands;
using Core.Contract.Application.Events;
using Core.Contract.RequestInfos;
using Core.UnitOfWorks;
using System.Text.Json;

namespace Core.Application;

public class CommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandHandler<T> _commandHandler;
    private readonly IEventBus _eventBus;
    private readonly IEventRepository _eventRepository;
    private readonly RequestInfoService _requestInfoService;

    public CommandHandlerDecorator(
        IUnitOfWork unitOfWork,
        ICommandHandler<T> commandHandler,
        IEventBus eventBus,
        IEventRepository eventRepository,
        RequestInfoService requestInfoService)
    {
        _unitOfWork = unitOfWork;
        _commandHandler = commandHandler;
        _eventBus = eventBus;
        _eventRepository = eventRepository;
        _requestInfoService = requestInfoService;
    }

    public async Task Handle(T command)
    {
        var eventModels = new List<EventModel>();

        _eventBus.Subscribe<Event>(e =>
        {
            var data = JsonSerializer.Serialize(e, e.GetType());

            eventModels.Add(new EventModel()
            {
                Data = data,
                EventName = e.GetType().Name,
                DateTime = DateTime.Now,
                UserName = _requestInfoService.UserName
            });
        });

        var handlerHasError = false;

        _eventBus.Subscribe<CollectionValidationsFailed>(e =>
        {
            handlerHasError = true;
            _unitOfWork.Rollback();
        });

        try
        {
            await _unitOfWork.BeginTransaction();

            await _commandHandler.Handle(command);

            if (!handlerHasError)
            {
                _eventRepository.Create(eventModels);
                await _unitOfWork.Commit();
            }
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }

    }
}

public class CommandHandlerWithOutputDecorator<T, T2> : ICommandHandler<T, T2> where T : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandHandler<T, T2> _commandHandler;
    private readonly IEventBus _eventBus;
    private readonly RequestInfoService _requestInfoService;

    public CommandHandlerWithOutputDecorator(IUnitOfWork unitOfWork,
        ICommandHandler<T, T2> commandHandler,
        IEventBus eventBus,
        RequestInfoService requestInfoService)
    {
        _unitOfWork = unitOfWork;
        _commandHandler = commandHandler;
        _eventBus = eventBus;
        _requestInfoService = requestInfoService;
    }

    public async Task<T2> Handle(T command)
    {
        var handlerHasError = false;

        _eventBus.Subscribe<CollectionValidationsFailed>(e =>
        {
            handlerHasError = true;
            _unitOfWork.Rollback();
        });

        try
        {
            await _unitOfWork.BeginTransaction();
            var result = await _commandHandler.Handle(command);
            if(!handlerHasError)
            {
                await _unitOfWork.Commit();
                return result;
            }

            throw new NotImplementedException();
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }

    }
}
