using Core.Contract.Mapper;

namespace Core.Application.Queries;

public class QueryHandlerService
{
    private readonly IMapperService _mapperService;

    public QueryHandlerService(IMapperService mapperService)
    {
        _mapperService = mapperService;
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
