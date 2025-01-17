using UnityEngine;

namespace Naninovel.U.MusicChainPlayer
{
    public interface IMusicChainPlayerManager : IEngineService
    {
        /// <summary>
        /// Write the body of your MusicChainPlayer interface here
        /// </summary>
        /// 
        public void PlayCBgm(string[] names);
        public void StopCBgm();
    }
}