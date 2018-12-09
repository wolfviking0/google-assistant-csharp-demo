using System;
namespace GAssistant.Config
{
    [Serializable]
    public class DeviceRegisterConf
    {
        public string apiEndpoint { get; set; }

        public string projectId { get; set; }

        public string deviceModelFilePath { get; set; }

        public string deviceInstanceFilePath { get; set; }
    }
}
