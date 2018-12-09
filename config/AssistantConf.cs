using System;
namespace GAssistant.Config
{
    [Serializable]
    public class AssistantConf
    {
        public string assistantApiEndpoint { get; set; }

        public int audioSampleRate { get; set; }

        public int chunkSize { get; set; }

        public int volumePercent { get; set; }

        public string languageCode { get; set; }
    }
}
