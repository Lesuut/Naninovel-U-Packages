using UnityEngine;
using UnityEngine.Audio;

namespace Naninovel.U.MusicChainPlayer
{
    /// <summary>
    /// Contains configuration data for the MusicChainPlayer systems.
    /// </summary>
    [EditInProjectSettings]
    public class MusicChainPlayerConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "MusicChainPlayer";

        /// <summary>
        /// Write here the body of the configuration for MusicChainPlayer.
        /// </summary>
        /// 
        public AudioMixer AudioMixer;
        public MusicItem[] MusicItems;
    }
}