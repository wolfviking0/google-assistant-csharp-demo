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

        }

        internal void register()
        {
            throw new NotImplementedException();
        }

        internal DeviceModel getDeviceModel()
        {
            return deviceModel;
        }

        internal Device getDevice()
        {
            return device;
        }
    }
}
