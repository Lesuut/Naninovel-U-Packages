using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    [System.Serializable]
    public struct UnlockableImages
    {
        public UnlockableImages(string key)
        {
            unlockableKey = key;
            sprite = null; // Инициализируем спрайт значением null
        }

        public Sprite sprite;
        public string unlockableKey;
    }
}