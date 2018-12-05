using System;
using System.IO;
using googleassistantcsharpdemo.config;
using Newtonsoft.Json;

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

            oAuthClient = new OAuthClient(authenticationConf.googleOAuthEndpoint);
        }

        public void authenticate()
        {
            try {
                if (File.Exists(authenticationConf.credentialsFilePath))
                {
                    Logger.Get().Debug("Loading oAuth credentials from file");

                    using (StreamReader file = File.OpenText(authenticationConf.credentialsFilePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        oAuthCredentials = (OAuthCredentials)serializer.Deserialize(file, typeof(OAuthCredentials));
                    }

                    Logger.Get().Debug("Access Token: " + oAuthCredentials.access_token);
                }
                else {
                    OAuthCredentials optCredentials = requestAccessToken();
                    if (optCredentials != null) {
                        oAuthCredentials = optCredentials;
                        Logger.Get().Debug("Access Token: " + oAuthCredentials.access_token);
                        saveCredentials(); 
                    } else {
                        throw new Exception("requestAccessToken error");
                    }
                }

            } catch (Exception e) {
                Logger.Get().Error("Error during authentication" + e);
            }
        }

        public bool expired()
        {
            return oAuthCredentials.expiration_time - DateTimeOffset.Now.ToUnixTimeMilliseconds() < authenticationConf.maxDelayBeforeRefresh;
        }

        public OAuthCredentials getOAuthCredentials()
        {
            return oAuthCredentials;
        }

        public OAuthCredentials refreshAccessToken() {
            Logger.Get().Debug("Refreshing access token");

            OAuthCredentials response = oAuthClient.refreshAccessToken(
                    oAuthCredentials.refresh_token,
                    authenticationConf.clientId,
                    authenticationConf.clientSecret,
                    "refresh_token");

            if (response != null) {
                Logger.Get().Debug("New Access Token: " + response.access_token);
                oAuthCredentials.access_token = response.access_token;
                oAuthCredentials.expires_in = response.expires_in;
                oAuthCredentials.token_type = response.token_type;
                saveCredentials();
                return oAuthCredentials;
            } else {
                return null;
            }
        }

        public OAuthCredentials requestAccessToken() {
            string url = "https://accounts.google.com/o/oauth2/v2/auth?" +
            "scope=" + authenticationConf.scope + "&" +
            "response_type=code&" +
            "redirect_uri=" + authenticationConf.codeRedirectUri + "&" +
            "client_id=" + authenticationConf.clientId;

            System.Diagnostics.Process.Start(url);

            Console.WriteLine("Allow the application in your browser and copy the authorization code in the console");
            Console.WriteLine("Enter authentification : ");
            string code = Console.ReadLine();

            OAuthCredentials response = oAuthClient.getAccessToken(
              code,
              authenticationConf.clientId,
              authenticationConf.clientSecret,
              authenticationConf.codeRedirectUri,
              "authorization_code");

            return response;
        }

        public void saveCredentials() {
            try
            {
                oAuthCredentials.expiration_time = DateTimeOffset.Now.ToUnixTimeMilliseconds() + oAuthCredentials.expires_in * 1000;
                using (StreamWriter file = File.CreateText(authenticationConf.credentialsFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, oAuthCredentials);
                }
            }
            catch (Exception e)
            {
                Logger.Get().Error("Error saving credentials" + e);
            }
        }
    }
}
