using System;
using googleassistantcsharpdemo.config;

namespace googleassistantcsharpdemo.device
{
    public class DeviceRegister
    {
        private DeviceRegisterConf deviceRegisterConf;

        private DeviceModel deviceModel;

        private Device device;

        private DeviceInterface deviceInterface;

        public DeviceRegister(DeviceRegisterConf deviceRegisterConf, string accessToken)
        {
            this.deviceRegisterConf = deviceRegisterConf;

            deviceInterface = new DeviceInterface(deviceRegisterConf.apiEndpoint, accessToken);
        }

        public void register()
        {
            string projectId = deviceRegisterConf.projectId;

            deviceModel = registerModel(projectId);
        }

        public DeviceModel getDeviceModel()
        {
            return deviceModel;
        }

        public Device getDevice()
        {
            return device;
        }

        private DeviceModel registerModel(string projectId) {
            return null;
        }
    }
}
