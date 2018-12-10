using System;
using System.IO;
using System.Text;
using System.Threading;
using GAssistant.Api;
using GAssistant.Authentication;
using GAssistant.Config;
using GAssistant.Device;
using Newtonsoft.Json;

namespace GAssistant
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Logger.Init();

            // LOAD REFERENCES CONF
            FactoryConf fc = null;
            using (StreamReader file = File.OpenText("resources/reference.conf"))
            {
                JsonSerializer serializer = new JsonSerializer();
                fc = (FactoryConf)serializer.Deserialize(file, typeof(FactoryConf));
            }

            // Authentication
            AuthenticationHelper authenticationHelper = new AuthenticationHelper(fc.authenticationConf);
            authenticationHelper.Authenticate().Wait();
            //authenticationHelper.AuthenticateWithInput();

            // Register Device model and device
            DeviceRegister deviceRegister = new DeviceRegister(fc.deviceRegisterConf, authenticationHelper.GetOAuthCredentials().access_token);
            deviceRegister.Register();

            // Build the client (stub)
            AssistantClient assistantClient = new AssistantClient(authenticationHelper.GetOAuthCredentials(), fc.authenticationConf, fc.assistantConf,
            deviceRegister.GetDeviceModel(), deviceRegister.GetDevice());

            // Main loop 
            bool isDone = false;
            while (!isDone)
            {
                // Check if we need to refresh the access token to request the api
                if (authenticationHelper.Expired())
                {
                    authenticationHelper.RefreshAccessToken();

                    assistantClient.UpdateCredentials(authenticationHelper.GetOAuthCredentials());
                }

                {
                    Logger.Get().Debug("Tap your request and press enter... (Tap quit to exit)");

                    string query = Console.ReadLine();

                    if (query.ToLower().Equals("quit")) break;
                    if (query.Length == 0) continue;

                    // requesting assistant with text query
                    assistantClient.TextRequestAssistant(query).Wait();

                    Logger.Get().Debug(">> " + assistantClient.GetTextResponse());
                    Logger.Get().Debug("   (AUDIO : " + (assistantClient.GetAudioResponse() != null ? assistantClient.GetAudioResponse().Length:0) + ")");
                }
            }

            Logger.Get().Debug("FINISH");
        }
    }
}
