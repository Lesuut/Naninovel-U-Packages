using Naninovel;
using Steamworks;
using UnityEngine;

namespace Steam
{
    [InitializeAtRuntime]
    public class SteamAchievement : ISteamAchievement
    {
        public UniTask InitializeServiceAsync()
        {
            //Debug.Log("Init SteamAchievement");
            return UniTask.CompletedTask;
        }

        public void ResetService() { /*Debug.Log("Reset SteamAchievement");*/ }

        public void DestroyService() { /*Debug.Log("Destroy SteamAchievement");*/ }

        public void SetAchievement(string achName)
        {
            if (!SteamAPI.IsSteamRunning() || !SteamManager.Initialized) return;

            SteamUserStats.SetAchievement(achName);
            SteamUserStats.StoreStats();
        }

        public void ClearAchievement(string achName)
        {
            if (!SteamAPI.IsSteamRunning() || !SteamManager.Initialized) return;

            SteamUserStats.ClearAchievement(achName);
            SteamUserStats.StoreStats();
        }
    }
}