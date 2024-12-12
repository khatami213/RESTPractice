using Core.Application;
using Core.Contract.Application.Commands;
using Core.Contract.Application.Events;
using Core.Contract.Factories;
using Core.Contract.RequestInfos;
using Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories;

public class CommandHandlerDecoratorFactory : ICommandHandlerDecoratorFactory
{
    private readonly ICommandHandlerFactory _commandHandlerFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IEventRepository _eventRepository;
    private readonly RequestInfoService _requestInfoService;

    public CommandHandlerDecoratorFactory(ICommandHandlerFactory commandHandlerFactory, 
        IUnitOfWork unitOfWork, 
        IEventBus eventBus, 
        IEventRepository eventRepository, 
        RequestInfoService requestInfoService)
    {
        _commandHandlerFactory = commandHandlerFactory;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _eventRepository = eventRepository;
        _requestInfoService = requestInfoService;
    }

    public ICommandHandler<T> Get<T>() where T : ICommand
    {
        var commandHandler = _commandHandlerFactory.Get<T>();
        var commandHandlerDecorator = new CommandHandlerDecorator<T>(_unitOfWork, commandHandler, _eventBus, _eventRepository, _requestInfoService);
        return commandHandlerDecorator;
    }

    public ICommandHandler<T1, T2> Get<T1, T2>() where T1 : ICommand
    {
        var commandHandler = _commandHandlerFactory.Get<T1, T2>();
        var commandHandlerDecorator = new CommandHandlerWithOutputDecorator<T1, T2>(_unitOfWork, commandHandler, _eventBus, _requestInfoService);
        return commandHandlerDecorator;
    }

    public IEnumerable<ICommandHandler<T>> GetAll<T>() where T : ICommand
    {
        throw new NotImplementedException();
    }
}
