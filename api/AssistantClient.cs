using System;
using googleassistantcsharpdemo.authentication;
using googleassistantcsharpdemo.config;
using googleassistantcsharpdemo.device;

using Google.Assistant.Embedded.V1Alpha1;
using Grpc.Core;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Newtonsoft.Json;

namespace googleassistantcsharpdemo.api
{
    public class AssistantClient
    {
        private EmbeddedAssistant.EmbeddedAssistantClient embeddedAssistantClient;

        private AuthenticationConf authenticationConf;
        private AssistantConf assistantConf;
        private DeviceModel deviceModel;
        private Device device;

        private Channel channel;
        private UserCredential credential;

        public AssistantClient(OAuthCredentials oAuthCredentials, AuthenticationConf authenticationConf, AssistantConf assistantConf, DeviceModel deviceModel, Device device)
        {
            this.authenticationConf = authenticationConf;
            this.assistantConf = assistantConf;
            this.deviceModel = deviceModel;
            this.device = device;

            GoogleAuthorizationCodeFlow googleAuthFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
            {
                ClientSecrets = new ClientSecrets()
                {
                    ClientId = authenticationConf.clientId,
                    ClientSecret = authenticationConf.clientSecret,
                }
            });

            TokenResponse responseToken = new TokenResponse()
            {
                AccessToken = oAuthCredentials.access_token,
                ExpiresInSeconds = oAuthCredentials.expires_in,
                RefreshToken = oAuthCredentials.refresh_token,
                Scope = authenticationConf.scope,
                TokenType = oAuthCredentials.token_type,
            };

            credential = new UserCredential(googleAuthFlow, "", responseToken);

            channel = new Channel(assistantConf.assistantApiEndpoint, 443, credential.ToChannelCredentials());

            embeddedAssistantClient = new EmbeddedAssistant.EmbeddedAssistantClient(channel);
        }

        public void updateCredentials(OAuthCredentials oAuthCredentials)
        {

        }

        public void requestAssistant(byte[] request)
        {

        }

        public string getTextResponse()
        {
            return "";
        }
    }
}
