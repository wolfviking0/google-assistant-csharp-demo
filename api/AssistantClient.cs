using System;
using GAssistant.Authentication;
using GAssistant.Config;
using GAssistant.Device;

using Google.Assistant.Embedded.V1Alpha2;
using Grpc.Core;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Newtonsoft.Json;
using Google.Protobuf;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace GAssistant.Api
{
    public class AssistantClient
    {
        private EmbeddedAssistant.EmbeddedAssistantClient embeddedAssistantClient;

        private AuthenticationConf authenticationConf;
        private AssistantConf assistantConf;
        private DeviceModel deviceModel;
        private DeviceDesc device;
        private GoogleAuthorizationCodeFlow googleAuthFlow;

        private ByteString currentConversationState = ByteString.Empty;
        private Channel channel;
        private UserCredential credential;

        IClientStreamWriter<AssistRequest> requestStream;
        IAsyncStreamReader<AssistResponse> responseStream;

        private byte[] currentAudioResponse;
        private string currentTextResponse;

        public AssistantClient(OAuthCredentials oAuthCredentials, AuthenticationConf authenticationConf, AssistantConf assistantConf, DeviceModel deviceModel, DeviceDesc device)
        {
            this.authenticationConf = authenticationConf;
            this.assistantConf = assistantConf;
            this.deviceModel = deviceModel;
            this.device = device;

            googleAuthFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
            {
                ClientSecrets = new ClientSecrets()
                {
                    ClientId = authenticationConf.clientId,
                    ClientSecret = authenticationConf.clientSecret,
                }
            });

            UpdateCredentials(oAuthCredentials);
        }

        public void UpdateCredentials(OAuthCredentials oAuthCredentials)
        {
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

        public async Task TextRequestAssistant(string request)
        {
            AsyncDuplexStreamingCall<AssistRequest, AssistResponse> assist = embeddedAssistantClient.Assist();

            requestStream = assist.RequestStream;
            responseStream = assist.ResponseStream;

            await requestStream.WriteAsync(GetConfigRequest(request));

            await WaitForResponse();
        }

        private async Task WaitForResponse()
        {
            var response = await responseStream.MoveNext();
            if (response)
            {
                AssistResponse currentResponse = responseStream.Current;
                OnNext(currentResponse);
            }
        }

        private void OnNext(AssistResponse value)
        {
            if (value.AudioOut != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write(value.AudioOut.AudioData.ToByteArray());
                    }
                    currentAudioResponse = stream.ToArray();
                }
            }

            if (value.DialogStateOut != null)
            {
                currentConversationState = value.DialogStateOut.ConversationState;

                if (!string.IsNullOrEmpty(value.DialogStateOut.SupplementalDisplayText))
                {

                    currentTextResponse = value.DialogStateOut.SupplementalDisplayText;
                }
            }
        }

        public byte[] GetAudioResponse()
        {
            return currentAudioResponse;
        }

        public string GetTextResponse()
        {
            return currentTextResponse;
        }

        private AssistRequest GetConfigRequest(string textQuery)
        {
            var audioInConfig = new AudioInConfig()
            {
                Encoding = AudioInConfig.Types.Encoding.Linear16,
                SampleRateHertz = assistantConf.audioSampleRate
            };

            var audioOutConfig = new AudioOutConfig()
            {
                Encoding = AudioOutConfig.Types.Encoding.Linear16,
                SampleRateHertz = assistantConf.audioSampleRate,
                VolumePercentage = assistantConf.volumePercent,
            };

            var dialogStateInConfig = new DialogStateIn()
            {
                // We set the us local as default
                LanguageCode = assistantConf.languageCode,
                ConversationState = currentConversationState
            };

            var deviceConfig = new DeviceConfig()
            {
                DeviceModelId = deviceModel.deviceModelId,
                DeviceId = device.id
            };

            var assistConfig = new AssistConfig()
            {
                AudioInConfig = audioInConfig,
                AudioOutConfig = audioOutConfig,
                DeviceConfig = deviceConfig,
                DialogStateIn = dialogStateInConfig,
                TextQuery = textQuery
            };

            AssistRequest assistRequest = new AssistRequest()
            {
                Config = assistConfig
            };

            return assistRequest;
        }
    }
}
