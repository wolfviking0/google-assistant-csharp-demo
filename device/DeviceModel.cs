using System;
namespace googleassistantcsharpdemo.device
{
    public class DeviceModel
    {
        public string deviceModelId;

        public string projectId;

        public Manifest manifest;

        public string name;

        public string deviceType;

        public class Manifest
        {
            public string manufacturer;

            public string productName;

            public string deviceDescription;
        }
    }
}
