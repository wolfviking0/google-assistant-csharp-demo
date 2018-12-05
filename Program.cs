using System;
using System.IO;
using googleassistantcsharpdemo.api;
using googleassistantcsharpdemo.authentication;
using googleassistantcsharpdemo.config;
using googleassistantcsharpdemo.device;
using Newtonsoft.Json;

namespace googleassistantcsharpdemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Logger.Init();

            // GENERATE THE XML CONF FILE
            //FactoryConf fc = new FactoryConf();
            //fc.assistantConf = new AssistantConf();
            //fc.audioConf = new AudioConf();
            //fc.authenticationConf = new AuthenticationConf();
            //fc.deviceRegisterConf = new DeviceRegisterConf();
            //fc.ioConf = new IoConf();
            //XmlSerializer xs = new XmlSerializer(typeof(FactoryConf));
            //FileStream fsout = new FileStream("reference.conf", FileMode.Create, FileAccess.Write, FileShare.None);
            //xs.Serialize(fsout, fc);

            // GENERATE THE JSON CONF FILE
            //using (StreamWriter file = File.CreateText("resources/reference_private.conf"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    serializer.Formatting = Formatting.Indented;
            //    serializer.Serialize(file, fc);
            //}

            // LOAD REFERENCES CONF
            FactoryConf fc = null;
            using (StreamReader file = File.OpenText("resources/reference_private.conf"))
            {
                JsonSerializer serializer = new JsonSerializer();
                fc = (FactoryConf)serializer.Deserialize(file, typeof(FactoryConf));
            }

            // Authentication
            AuthenticationHelper authenticationHelper = new AuthenticationHelper(fc.authenticationConf);
            authenticationHelper.authenticate();

            // Register Device model and device
            DeviceRegister deviceRegister = new DeviceRegister(fc.deviceRegisterConf, authenticationHelper.getOAuthCredentials().access_token);
            deviceRegister.register();


            // Build the client (stub)
            //AssistantClient assistantClient = new AssistantClient(authenticationHelper.getOAuthCredentials(), fc.assistantConf,
            //deviceRegister.getDeviceModel(), deviceRegister.getDevice(), fc.ioConf);

        }
    }
}
