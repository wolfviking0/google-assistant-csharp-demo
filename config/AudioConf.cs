using System;
namespace GAssistant.Config
{
    [Serializable]
    public class AudioConf
    {
        public int sampleRate { get; set; }

        public int sampleSizeInBits { get; set; }

        public int channels { get; set; }

        public bool signed { get; set; }

        public bool bigEndian { get; set; }
    }
}
