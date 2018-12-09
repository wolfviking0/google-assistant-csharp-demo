using System;
namespace GAssistant.Authentication
{
    [Serializable]
    public class OAuthCredentials
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string token_type { get; set; }

        public string refresh_token { get; set; }

        public string id_token { get; set; }

        public long expiration_time { get; set; }
    }
}
