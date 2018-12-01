using System;
namespace googleassistantcsharpdemo.config
{
    [Serializable]
    public class IoConf
    {
        public static readonly string TEXT = "TEXT";

        public static readonly string AUDIO = "AUDIO";

        public string inputMode { get; set; }

        public bool outputAudio { get; set; }
    }
}
