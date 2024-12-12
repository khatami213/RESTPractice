using Core.Contract.Mapper;
using Mapster;

namespace Core.Infrastructure.Mapper;

public class MapsterMapperService : IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>();
    }

    public void Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        source.Adapt(destination);
    }

    public object Map(object source, Type sourceType, Type destinationType)
    {
        return source.Adapt(sourceType, destinationType);
    }
}
