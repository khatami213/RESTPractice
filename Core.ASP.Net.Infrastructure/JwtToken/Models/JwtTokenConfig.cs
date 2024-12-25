using System.Text.Json.Serialization;

namespace Core.ASP.Net.Infrastructure.JwtToken.Models;

public class JwtTokenConfig
{
    [JsonPropertyName("secret")]
    public string Secret { get; set; }

    [JsonPropertyName("issuer")]
    public string Issuer { get; set; }

    [JsonPropertyName("audience")]
    public List<string> Audiences { get; set; }

    [JsonPropertyName("accessTokenExpiration")]
    public int AccessTokenExpiration { get; set; }

    [JsonPropertyName("refreshTokenExpiration")]
    public int RefreshTokenExpiration { get; set; }
}
