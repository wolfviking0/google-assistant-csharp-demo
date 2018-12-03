using System;
using System.IO;
using System.Xml.Serialization;
using googleassistantcsharpdemo.api;
using googleassistantcsharpdemo.authentication;
using googleassistantcsharpdemo.config;
using googleassistantcsharpdemo.device;

namespace googleassistantcsharpdemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Logger.Init();

            // GENERATE THE CONF FILE
            //FactoryConf fc = new FactoryConf();
            //fc.assistantConf = new AssistantConf();
            //fc.audioConf = new AudioConf();
            //fc.authenticationConf = new AuthenticationConf();
            //fc.deviceRegisterConf = new DeviceRegisterConf();
            //fc.ioConf = new IoConf();
            //XmlSerializer xs = new XmlSerializer(typeof(FactoryConf));
            //FileStream fsout = new FileStream("reference.conf", FileMode.Create, FileAccess.Write, FileShare.None);
            //xs.Serialize(fsout, fc);

            // LOAD REFERENCES CONF
            XmlSerializer xs = new XmlSerializer(typeof(FactoryConf));
            FileStream fsin = new FileStream("resources/reference_private.conf", FileMode.Open, FileAccess.Read, FileShare.None);
            FactoryConf fc = (FactoryConf)xs.Deserialize(fsin);

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
