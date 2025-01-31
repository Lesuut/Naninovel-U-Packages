using System;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel.U.MusicChainPlayer
{
    [Serializable]
    public class MusicChainPlayerState
    {
        public string[] Keys;

        public MusicChainPlayerState()
        {
            // Initialization Values
        }

        public MusicChainPlayerState(MusicChainPlayerState other)
        {
            // Load and set Data
            Keys = other.Keys;
        }
    }

    [Serializable]
    public class MusicItem
    {
        public string Key;
        public AudioClip AudioClip;
    }
}
