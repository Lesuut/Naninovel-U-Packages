using System;

namespace Naninovel.U.LibraryAudio
{
    [Serializable]
    public class AudioLibraryCurrentUseAudioItem
    {
        public string Key = "";
        public string Group = "";
        public bool Loop = false;
        public float Valume = AudioLibraryConfiguration.defSfxVolume;
    }
}
