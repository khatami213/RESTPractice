using Authorization.Query.Contract.Dtos.Users;
using Core.Contract.Application.Queries;
using Core.DataAnnotations;

namespace Authorization.Query.Contract.Queries.Users;

public class GetUserById : IQuery<UserDto>
{
    [CustomRequired]
    public long Id { get; set; }
}
