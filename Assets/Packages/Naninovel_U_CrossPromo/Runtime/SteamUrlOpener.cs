using UnityEngine;
using Steamworks;

namespace Naninovel.U.CrossPromo
{
    public static class SteamUrlOpener
    {
        /// <summary>
        /// Открывает указанный URL в Steam, если Steam инициализирован, иначе в браузере.
        /// </summary>
        /// <param name="url">Ссылка, которую нужно открыть.</param>
        public static void OpenUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("SteamUrlOpener: URL не может быть пустым.");
                return;
            }

            TryOpenUrlInSteam(url);
        }

        public static void TryOpenUrlInSteam(string url)
        {
            if (SteamManager.Initialized)
                SteamFriends.ActivateGameOverlayToWebPage(url);
            else
                Application.OpenURL(url);
        }
    }
}