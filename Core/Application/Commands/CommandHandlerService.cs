using Core.Contract.Application.Events;
using Core.Contract.Mapper;

namespace Core.Application.Commands;

public sealed class CommandHandlerService 
{
    private readonly IEventBus _eventBus;
    private readonly IMapperService _mapperService;

    public CommandHandlerService(IEventBus eventBus, IMapperService mapperService)
    {
        _eventBus = eventBus;
        _mapperService = mapperService;
    }

    public void Publish<T>(T e) where T : Event
    {
        _eventBus.Publish(e);
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return _mapperService.Map<TSource, TDestination>(source);
    }

    public void Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        _mapperService.Map(source, destination);
    }

    public object Map(object source, Type sourceType, Type destinationType)
    {
        return _mapperService.Map(source, sourceType, destinationType);
    }

}
