using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    [System.Serializable]
    public struct UnlockableImages
    {
        public UnlockableImages(string key, string leaderBoardKey)
        {
            unlockableKey = key;
            sprite = null;
            this.leaderBoardKey = leaderBoardKey;
        }

        public Sprite sprite;
        public string unlockableKey;
        public string leaderBoardKey;
    }
}