namespace Core.Contract.Mapper;

public interface IMapperService
{
    TDestination Map<TSource, TDestination>(TSource source);
    object Map(object source, Type sourceType, Type destinationType);
    void Map<TSource, TDestination>(TSource source, TDestination destination);
}
