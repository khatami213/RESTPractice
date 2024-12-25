using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Domain.Models.ReadModels.Users;

public class UserProjectReadModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public UserReadModel User { get; set; }
    public long ProjectId { get; set; }
    public ProjectReadModel Project { get; set; }
}
