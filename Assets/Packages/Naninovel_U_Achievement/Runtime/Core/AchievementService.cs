using Steamworks;
using System;

namespace Naninovel.U.Achievement
{
    [InitializeAtRuntime()]
    public class AchievementService : IAchievementService, IStatefulService<GlobalStateMap>
    {
        [Serializable]
        public class GlobalState
        {
            public AchievementState AchievementState = new AchievementState();
        }

        private readonly IStateManager stateManager;
        private AchievementState achievementState;

        public AchievementService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            achievementState = new AchievementState();

            return UniTask.CompletedTask;
        }

        public void DestroyService() { }

        public void ResetService() { }

        public virtual void SaveServiceState(GlobalStateMap stateMap)
        {
            var globalState = new GlobalState
            {
                AchievementState = new AchievementState(achievementState)
            };
            stateMap.SetState(globalState);
        }

        public virtual UniTask LoadServiceStateAsync(GlobalStateMap stateMap)
        {
            var state = stateMap.GetState<GlobalState>();
            if (state is null) return UniTask.CompletedTask;

            achievementState = achievementState == null ? new AchievementState() : new AchievementState(state.AchievementState);

            return UniTask.CompletedTask;
        }

        public void UnlockAchievement(string key)
        {
            if (!SteamAPI.IsSteamRunning() || !SteamManager.Initialized) return;

            SteamUserStats.SetAchievement(key);
            SteamUserStats.StoreStats();

            UnityEngine.Debug.Log($"Ach: <b><color=green>{key}</color></b>");

            if (!achievementState.RegisterAchievementActivatedKeys.Contains(key))
            {
                achievementState.RegisterAchievementActivatedKeys.Add(key);
                stateManager.SaveGlobalAsync().Forget();
            }
        }

        public bool IsAchievementGranted(string key)
        {
            if (!SteamAPI.IsSteamRunning() || !SteamManager.Initialized) return false;

            bool achieved = false;
            SteamUserStats.GetAchievement(key, out achieved);
            return achieved;
        }

        public void ResetAchievement(string key)
        {
            if (!SteamAPI.IsSteamRunning() || !SteamManager.Initialized) return;

            if (IsAchievementGranted(key))
            {
                SteamUserStats.ClearAchievement(key);
                SteamUserStats.StoreStats();

                UnityEngine.Debug.Log($"Achievement '<b><color=red>{key}</color></b>' Reset!");
            }
        }

        public void ResetAllAchievement()
        {
            if (!SteamAPI.IsSteamRunning() || !SteamManager.Initialized) return;

            foreach (var key in achievementState.RegisterAchievementActivatedKeys)
            {
                ResetAchievement(key);
            }
        }

        private void DebugState()
        {
            UnityEngine.Debug.Log($"State: {string.Join(", ", achievementState.RegisterAchievementActivatedKeys)}");
        }
    }
}
