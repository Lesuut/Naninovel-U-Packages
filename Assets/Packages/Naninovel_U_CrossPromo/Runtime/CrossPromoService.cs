using System;
using System.Collections.Generic;
using System.Linq;

namespace Naninovel.U.CrossPromo
{
    [InitializeAtRuntime()]
    public class CrossPromoService : ICrossPromoService, IStatefulService<GlobalStateMap>
    {
        [Serializable]
        public class GlobalState
        {
            public CrossPromoState CrossPromoState = new CrossPromoState();
        }

        public virtual CrossPromoConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private CrossPromoState crossPromoState;

        private IUIManager uiManager;
        private IUnlockableManager unlockableManager;

        private SheetData[] sheetDatas;

        public CrossPromoService(CrossPromoConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public async UniTask InitializeServiceAsync()
        {
            UnityEngine.Debug.Log("InitializeServiceAsync");

            if (!Configuration.crossPromoEnable)
                return;

            crossPromoState = new CrossPromoState();
            uiManager = Engine.GetService<IUIManager>();
            unlockableManager = Engine.GetService<IUnlockableManager>();

            try
            {
                sheetDatas = await GoogleSheetDataLoader.LoadDataAsync(Configuration.GoogleSheetDataURL, Configuration.debug);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"CrossPromo: Error loading data: {ex.Message}");
            }
        }

        public void DestroyService() { }
        public void ResetService() { }

        public virtual void SaveServiceState(GlobalStateMap stateMap)
        {
            var globalState = new GlobalState
            {
                CrossPromoState = new CrossPromoState(crossPromoState)
            };
            stateMap.SetState(globalState);
        }

        public virtual UniTask LoadServiceStateAsync(GlobalStateMap stateMap)
        {
            var state = stateMap.GetState<GlobalState>();
            if (state is null) return UniTask.CompletedTask;

            crossPromoState = crossPromoState == null ? new CrossPromoState() : new CrossPromoState(state.CrossPromoState);

            return UniTask.CompletedTask;
        }

        public void ShowCrossPromo(LinkTransitionType linkTransitionType)
        {
            if (!Configuration.crossPromoEnable)
            {
                UnityEngine.Debug.LogWarning("Cross Promo is Disable!");
                return;
            }

            if (Configuration.showAllSlotsAtStart)
            {
                for (int i = 0; i < sheetDatas.Length; i++)
                    UnlockItem(i);
            }

            if (crossPromoState.availableIdSlots.Count <= 0)
                crossPromoState.availableIdSlots.Add(0);

            RemoveDuplicates(ref crossPromoState.availableIdSlots);

            if (Configuration.debug)
            {
                UnityEngine.Debug.Log($"ShowCrossPromo sheetDatas count: {sheetDatas.Length}");
                UnityEngine.Debug.Log($"ShowCrossPromo availableIdSlots: {string.Join(", ", crossPromoState.availableIdSlots)}");
                UnityEngine.Debug.Log($"ShowCrossPromo receivedIdSlots: {string.Join(", ", crossPromoState.receivedIdSlots)}");

                foreach (var item in Configuration.unlockableImages)
                    UnityEngine.Debug.Log($"{item.unlockableKey}: {unlockableManager.ItemUnlocked(item.unlockableKey)}");
            }

            if (sheetDatas == null || sheetDatas.Length <= 0)
                throw new InvalidOperationException("CrossPromo: sheetDatas not initialized or empty!");

            var crossPromoUi = uiManager.GetUI<CrossPromoUI>();

            crossPromoUi.ClearSlots();

            foreach (var ID in crossPromoState.availableIdSlots)
            {
                if (ID <= sheetDatas.Length - 1)
                {
                    crossPromoUi.SpawnSlot(sheetDatas[ID].Image, () =>
                    {
                        if (!crossPromoState.receivedIdSlots.Contains(ID))
                        {
                            crossPromoUi.ShowContinueWindow(() =>
                            {
                                crossPromoState.availableIdSlots.Add(ID);
                                crossPromoState.receivedIdSlots.Add(ID);

                                unlockableManager.UnlockItem(Configuration.unlockableImages[ID].unlockableKey);
                                stateManager.SaveGlobalAsync().Forget();

                                UpdateSlotStatus();
                                UnityEngine.Debug.Log($"UnlockItem: {Configuration.unlockableImages[ID].unlockableKey}");

                                TryGetAchievement();

                                crossPromoUi.ShowAdult(Configuration.unlockableImages[ID].adultStatic);
                            });
                        }

                        LeaderBoardCoroutines.Instance.UpdateScore(sheetDatas[ID].LeaderBoardKey, 1);
                        LeaderBoardAddScore(linkTransitionType);
                        SteamUrlOpener.OpenUrl(sheetDatas[ID].Url);
                    }, ID);
                }
            }

            UpdateSlotStatus();

            crossPromoUi.Show();
        }

        public void UnlockItem(int id)
        {
            if (!Configuration.crossPromoEnable) return;
            if (!crossPromoState.availableIdSlots.Contains(id))
            {
                crossPromoState.availableIdSlots.Add(id);
                stateManager.SaveGlobalAsync().Forget();
            }
        }

        public void UnlockRandomItem()
        {
            if (!Configuration.crossPromoEnable) return;
            if (sheetDatas == null && crossPromoState.availableIdSlots.Count == sheetDatas.Length) return;

            List<int> availableIds = Enumerable.Range(0, sheetDatas.Length)
                                               .Except(crossPromoState.availableIdSlots)
                                               .ToList();

            if (availableIds.Count == 0) return;

            int rndId = availableIds[UnityEngine.Random.Range(0, availableIds.Count)];

            crossPromoState.availableIdSlots.Add(rndId);

            stateManager.SaveGlobalAsync().Forget();
        }

        public bool IsCGSlotValid(string unlockableKey)
        {
            int index = Array.FindIndex(Configuration.unlockableImages, item => item.unlockableKey == unlockableKey);

            if (index == -1) return false;

            return (Math.Clamp(index - 1, 0, int.MaxValue)) <= sheetDatas.Length - 2;
        }

        public bool IsCrossPromoEnabled() => Configuration.crossPromoEnable;

        private void UpdateSlotStatus()
        {
            var crossPromoUi = uiManager.GetUI<CrossPromoUI>();

            foreach (var item in crossPromoUi.SlotItems)
            {
                item.SetReceivedStatus(crossPromoState.receivedIdSlots.Contains(item.ID));
            }
        }

        private void RemoveDuplicates(ref List<int> list)
        {
            HashSet<int> seen = new HashSet<int>();
            list.RemoveAll(x => !seen.Add(x));
        }

        private void TryGetAchievement()
        {
            if (crossPromoState.receivedIdSlots.Count == sheetDatas.Length)
                PlayScript(Configuration.achievementNaniCommand);
        }

        // Проигрывает команду ачивки асинхронно. Если это не работает в вашей версии, нужно выдрать код из класса PlayScript (namespace Naninovel)
        // Или попробовать раскомитить код ниже
        private async void PlayScript(string scriptText)
        {
            var script = Script.FromScriptText($"Generated script", scriptText);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }

        /*protected virtual void PlayScript(string scriptText)
        {
            var player = Engine.GetService<IScriptPlayer>();
            player.PlayTransient($"`Cross Promo` generated script", scriptText).Forget();
        }*/

        private void LeaderBoardAddScore(LinkTransitionType linkTransitionType)
        {
            switch (linkTransitionType)
            {
                case LinkTransitionType.Gallery:
                    LeaderBoardCoroutines.Instance.UpdateScore(Configuration.GalleryLeaderBoardCrossPromoClickKey, 1);
                    break;
                case LinkTransitionType.Menu:
                    LeaderBoardCoroutines.Instance.UpdateScore(Configuration.MenuLeaderBoardCrossPromoMenuClickKey, 1);
                    break;
                case LinkTransitionType.Final:
                    LeaderBoardCoroutines.Instance.UpdateScore(Configuration.FinalLeaderBoardCrossPromoClickKey, 1);
                    break;
            }
        }
    }
}
