using System;
namespace googleassistantcsharpdemo.config
{
    [Serializable]
    public class AssistantConf
    {
        public string assistantApiEndpoint { get; set; }

        public int audioSampleRate { get; set; }

        public int chunkSize { get; set; }

        public int volumePercent { get; set; }
    }
}
