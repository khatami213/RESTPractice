namespace Core.Contract.Config;

public class AuthorizeConfigOptions
{
    public string AuthorizeType { get; set; }

    public IdentityServerConfig? IdentityServer { get; set; }
}

public class IdentityServerConfig
{
    public string? ServerUrl { get; set; }
}
