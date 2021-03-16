using System.Text.Json.Serialization;

namespace MyTransfer.Application.Common
{
    public class ApplicationSettings
    {
        [JsonPropertyName("jwt")]
        public JwtTokenConfig JWT { get; set; }

        [JsonPropertyName("mailtrap")]
        public MailtrapConfig Mailtrap { get; set; }

        public class JwtTokenConfig
        {
            [JsonPropertyName("secret")]
            public string Secret { get; set; }

            [JsonPropertyName("issuer")]
            public string Issuer { get; set; }

            [JsonPropertyName("audience")]
            public string Audience { get; set; }

            [JsonPropertyName("accessTokenExpiration")]
            public int AccessTokenExpiration { get; set; }

            [JsonPropertyName("refreshTokenExpiration")]
            public int RefreshTokenExpiration { get; set; }
        }

        public class MailtrapConfig
        {
            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonPropertyName("password")]
            public string Password { get; set; }

            [JsonPropertyName("host")]
            public string Host { get; set; }

            [JsonPropertyName("port")]
            public int? Port { get; set; }

            [JsonPropertyName("emailfrom")]
            public string DefaultFromEmail { get; set; }
        }
    }
}
