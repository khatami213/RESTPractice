using Authorization.Application.Services.CommandHandlers.Accounts;
using Authorization.Application.Services.CommandHandlers.Users;
using Authorization.Application.Services.QueryHandlers.Users;
using Core.Application;
using Core.Application.Commands;
using Core.Application.Events;
using Core.Application.Facade.Commands;
using Core.Application.Facade.Queries;
using Core.Application.Queries;
using Core.Application.Validations;
using Core.Contract.Application.Commands;
using Core.Contract.Application.Events;
using Core.Contract.Application.Queries;
using Core.Contract.Application.Validations;
using Core.Contract.Errors;
using Core.Contract.Facade;
using Core.Contract.Factories;
using Core.Contract.Mapper;
using Core.Contract.RequestInfos;
using Core.Contract.ServiceLocators;
using Core.EF.Infrastracture.Application.Events;
using Core.EF.Infrastracture.EF;
using Core.EF.Infrastracture.UnitOfWorks;
using Core.Factories;
using Core.Infrastructure.Mapper;
using Core.Infrastructure.ServiceLocators;
using Core.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.Config.Startup;

public static class ServiceConfig
{
    public static void AddRequiredServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceLocator,ServiceLocator>();
        services.AddScoped<RequestInfoService>();
        services.AddScoped<IFacadeCommandService, FacadeCommandService>();
        services.AddScoped<IFacadeQueryService, FacadeQueryService>();
        services.AddScoped<CommandHandlerService>();
        services.AddScoped<QueryHandlerService>();
        services.AddScoped<IMapperService,MapsterMapperService>();
        services.AddScoped<IMapperError,MapperError>();
        services.AddScoped<IEventBus, EventBus>();
        services.AddScoped<IValidator, Validator>();
        services.AddScoped<ICommandBus, CommandBus>();
        services.AddScoped<IQueryBus, QueryBus>();
        services.AddScoped<ICommandHandlerFactory, CommandHandlerFactory>();
        services.AddScoped<IQueryHandlerFactory, QueryHandlerFactory>();
        services.AddScoped<ICommandHandlerDecoratorFactory, CommandHandlerDecoratorFactory>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>(sp =>
        {
            var context = sp.GetRequiredService<CoreDbContext>();
            return new UnitOfWork(context);
        });


        //var assembly = typeof(CreateUserHandler).Assembly;
        //var handlerTypes = assembly.GetTypes()
        //    .Where(t => !t.IsAbstract && !t.IsInterface
        //        && t.GetInterfaces().Any(i => i.IsGenericType
        //            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

        //foreach (var handlerType in handlerTypes)
        //{
        //    var handlerInterface = handlerType.GetInterfaces()
        //        .First(i => i.IsGenericType
        //            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

        //    services.AddScoped(handlerInterface, handlerType);
        //}
        //services.TryAddScoped(typeof(ICommandHandler<>), typeof(CommandHandlerDecorator<>));

        //assembly = typeof(LoginHandler).Assembly;
        //handlerTypes = assembly.GetTypes()
        //    .Where(t => !t.IsAbstract && !t.IsInterface
        //        && t.GetInterfaces().Any(i => i.IsGenericType
        //            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

        //foreach (var handlerType in handlerTypes)
        //{
        //    var handlerInterface = handlerType.GetInterfaces()
        //        .First(i => i.IsGenericType
        //            && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

        //    services.AddScoped(handlerInterface, handlerType);
        //}
        //services.TryAddScoped(typeof(ICommandHandler<,>), typeof(CommandHandlerWithOutputDecorator<,>));

        services.TryDecorate(typeof(ICommandHandler<,>), typeof(CommandHandlerWithOutputDecorator<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerDecorator<>));

        services.Scan(scan => scan
                             .FromAssemblyOf<CreateUserHandler>()
                             .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                             .AsImplementedInterfaces()
                             .WithScopedLifetime());

        services.Scan(scan => scan
                             .FromAssemblyOf<LoginHandler>()
                             .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                             .AsImplementedInterfaces()
                             .WithScopedLifetime());

        services.TryAddScoped(typeof(ICollectionValidationHandler<>),typeof(CollectionValidationHandler<>));
        services.TryAddScoped(typeof(ICollectionValidationHandler<,>), typeof(CollectionValidationHandler<,>));

        services.AddScoped<List<ValidationCollectionError>>();

        var assembly = typeof(GetUserByIdHandler).Assembly;
        var handlerTypes = assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType &&
                     i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

        foreach (var handlerType in handlerTypes)
        {
            var handlerInterface = handlerType.GetInterfaces()
                .First(i => i.IsGenericType &&
                           i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            services.AddScoped(handlerInterface, handlerType);
        }

    }
}
