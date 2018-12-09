using System;
namespace GAssistant.Config
{
    [Serializable]
    public class FactoryConf
    {
        public AssistantConf assistantConf { get; set; }
	
        public AudioConf audioConf { get; set; }
	
        public AuthenticationConf authenticationConf { get; set; }
	
        public DeviceRegisterConf deviceRegisterConf { get; set; }
    }
}
