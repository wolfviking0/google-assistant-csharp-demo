using System;
using googleassistantcsharpdemo.config;

namespace googleassistantcsharpdemo.authentication
{
    public class AuthenticationHelper
    {
        private AuthenticationConf authenticationConf;

        private OAuthCredentials oAuthCredentials;

        private OAuthClient oAuthClient;

        public AuthenticationHelper(AuthenticationConf authenticationConf)
        {
            this.authenticationConf = authenticationConf;
        }

        internal void authenticate()
        {

        }

        internal OAuthCredentials getOAuthCredentials()
        {
            return oAuthCredentials;
        }
    }
}
