using System;

namespace Naninovel.U.LibraryAudio
{
    [Serializable]
    public struct AudioLibraryGroupItem
    {
        public string GroupName;
        public AudioLibraryItem[] AudioItems;
    }
}