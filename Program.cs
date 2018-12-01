using System;
using System.IO;
using System.Xml.Serialization;
using googleassistantcsharpdemo.config;


namespace googleassistantcsharpdemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Logger logger = new Logger();

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
            FileStream fsin = new FileStream("resources/reference.conf", FileMode.Open, FileAccess.Read, FileShare.None);
            FactoryConf fc = (FactoryConf)xs.Deserialize(fsin);

            logger.Debug("AL -- " + fc.assistantConf.chunkSize);
        }
    }
}
