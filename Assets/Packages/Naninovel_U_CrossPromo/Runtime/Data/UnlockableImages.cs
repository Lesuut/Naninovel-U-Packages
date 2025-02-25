using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    [System.Serializable]
    public struct UnlockableImages
    {
        public UnlockableImages(string key)
        {
            unlockableKey = key;
            adultStatic = null;
        }

        public Sprite adultStatic;
        public string unlockableKey;
    }
}