using System;
using System.IO;
using GAssistant.Config;
using Newtonsoft.Json;

namespace GAssistant.Authentication
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

        public void Authenticate()
        {
            try {
                if (File.Exists(authenticationConf.credentialsFilePath))
                {
                    using (StreamReader file = File.OpenText(authenticationConf.credentialsFilePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        oAuthCredentials = (OAuthCredentials)serializer.Deserialize(file, typeof(OAuthCredentials));
                    }
                }
                else {
                    OAuthCredentials optCredentials = RequestAccessToken();
                    if (optCredentials != null) {
                        oAuthCredentials = optCredentials;
                        SaveCredentials(); 
                    } else {
                        Logger.Get().Error("Request AccessToken Error");
                    }
                }

            } catch (Exception e) {
                Logger.Get().Error("Error during Authenticate : " + e);
            }
        }

        public bool Expired()
        {
            return oAuthCredentials.expiration_time - DateTimeOffset.Now.ToUnixTimeMilliseconds() < authenticationConf.maxDelayBeforeRefresh;
        }

        public OAuthCredentials GetOAuthCredentials()
        {
            return oAuthCredentials;
        }

        public OAuthCredentials RefreshAccessToken() {
            OAuthCredentials response = oAuthClient.RefreshAccessToken(
                    oAuthCredentials.refresh_token,
                    authenticationConf.clientId,
                    authenticationConf.clientSecret,
                    "refresh_token");

            if (response != null) {
                oAuthCredentials.access_token = response.access_token;
                oAuthCredentials.expires_in = response.expires_in;
                oAuthCredentials.token_type = response.token_type;
                SaveCredentials();
                return oAuthCredentials;
            } else {
                return null;
            }
        }

        private OAuthCredentials RequestAccessToken() {
            string url = "https://accounts.google.com/o/oauth2/v2/auth?" +
            "scope=" + authenticationConf.scope + "&" +
            "response_type=code&" +
            "redirect_uri=" + authenticationConf.codeRedirectUri + "&" +
            "client_id=" + authenticationConf.clientId;

            System.Diagnostics.Process.Start(url);

            Logger.Get().Debug("Allow the application in your browser and copy the authorization code in the console");
            Logger.Get().Debug("Enter authentification : ");
            string code = Console.ReadLine();

            OAuthCredentials response = oAuthClient.GetAccessToken(
              code,
              authenticationConf.clientId,
              authenticationConf.clientSecret,
              authenticationConf.codeRedirectUri,
              "authorization_code");

            return response;
        }

        private void SaveCredentials() {
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
                Logger.Get().Error("Error during SaveCredentials : " + e);
            }
        }
    }
}
