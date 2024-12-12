using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.ASP.Net.Infrastructure.Authorizations;

public class AuthenticatedUser : IIdentity
{
    public string AuthenticationType { get; }

    public bool IsAuthenticated { get; }

    public string Name { get; }

    public AuthenticatedUser(string authenticationType, bool isAuthenticated, string name)
    {
        AuthenticationType = authenticationType;
        IsAuthenticated = isAuthenticated;
        Name = name;
    }
}
