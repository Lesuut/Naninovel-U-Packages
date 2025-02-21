using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    [System.Serializable]
    public struct UnlockableImages
    {
        public UnlockableImages(string key, string leaderBoardKey)
        {
            unlockableKey = key;
            adultStatic = null;
            this.leaderBoardKey = leaderBoardKey;
        }

        public Sprite adultStatic;
        public string unlockableKey;
        public string leaderBoardKey;
    }
}