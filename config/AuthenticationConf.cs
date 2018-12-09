using System;
namespace GAssistant.Config
{
    [Serializable]
    public class AuthenticationConf
    {
        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string scope { get; set; }

        public string codeRedirectUri { get; set; }

        public string googleOAuthEndpoint { get; set; }

        public string urlGoogleAccount { get; set; }

        public string credentialsFilePath { get; set; }

        public long maxDelayBeforeRefresh { get; set; }
    }
}
