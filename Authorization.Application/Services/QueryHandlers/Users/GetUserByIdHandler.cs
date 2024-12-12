using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories;
using Authorization.Query.Contract.Dtos.Users;
using Authorization.Query.Contract.Queries.Users;
using Core.Application.Queries;
using Core.Contract.Application.Queries;
using Core.Contract.RequestInfos;

namespace Authorization.Application.Services.QueryHandlers.Users;

public class GetUserByIdHandler : IQueryHandler<GetUserById, UserDto>
{
    private readonly QueryHandlerService _queryHandlerService;
    private readonly IUserReadRepository _userReadRepository;
    private readonly RequestInfoService _requestInfoService;

    public GetUserByIdHandler(QueryHandlerService queryHandlerService, 
        IUserReadRepository userReadRepository, 
        RequestInfoService requestInfoService)
    {
        _queryHandlerService = queryHandlerService;
        _userReadRepository = userReadRepository;
        _requestInfoService = requestInfoService;
    }

    public async Task<UserDto> Handle(GetUserById query)
    {
        var user = await _userReadRepository.GetById(query.Id, x => x.Roles);
        return _queryHandlerService.Map<UserReadModel, UserDto>(user);
    }
}
