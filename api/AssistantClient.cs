using System;
using googleassistantcsharpdemo.authentication;
using googleassistantcsharpdemo.config;
using googleassistantcsharpdemo.device;

namespace googleassistantcsharpdemo.api
{
    public class AssistantClient
    {
        private AssistantConf assistantConf;

        private IoConf ioConf;

        public AssistantClient(OAuthCredentials oAuthCredentials, AssistantConf assistantConf, DeviceModel deviceModel, Device device, IoConf ioConf)
        {
            this.assistantConf = assistantConf;
            this.ioConf = ioConf;
        }
    }
}
