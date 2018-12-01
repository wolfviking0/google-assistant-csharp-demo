using System;
namespace googleassistantcsharpdemo.config
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
