using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        private GoogleSheetDataLoader googleSheetDataLoader;
        private SheetData[] sheetDatas;

        public CrossPromoService(CrossPromoConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public async UniTask InitializeServiceAsync()
        {
            googleSheetDataLoader = new GoogleSheetDataLoader();
            crossPromoState = new CrossPromoState();
            uiManager = Engine.GetService<IUIManager>();
            unlockableManager = Engine.GetService<IUnlockableManager>();

            try
            {
                sheetDatas = await googleSheetDataLoader.LoadDataAsync(Configuration.GoogleSheetDataURL);
            }
            catch (Exception ex)
            {
                Debug.LogError($"CrossPromo: Error loading data: {ex.Message}");
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

        public void ShowCrossPromo()
        {
            if (crossPromoState.availableIdSlots.Count <= 0)
                crossPromoState.availableIdSlots.Add(0);

            RemoveDuplicates(ref crossPromoState.availableIdSlots);

            //Debug.Log($"ShowCrossPromo: {string.Join(", ", crossPromoState.availableIdSlots)}");

            if (sheetDatas == null || sheetDatas.Length <= 0)
                throw new InvalidOperationException("CrossPromo: sheetDatas not initialized or empty!");

            if (sheetDatas.Length != Configuration.unlockableImages.Length)
                throw new InvalidOperationException("CrossPromo: The number of unlockableImages in the configuration must be equal to the number of items in the table!");

            var crossPromoUi = uiManager.GetUI<CrossPromoUI>();

            crossPromoUi.ClearSlots();

            foreach (var ID in crossPromoState.availableIdSlots)
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
                            Debug.Log($"UnlockItem: {Configuration.unlockableImages[ID].unlockableKey}");

                            crossPromoUi.ShowAdult(Configuration.unlockableImages[ID].sprite);
                        });
                    }

                    SteamUrlOpener.OpenUrl(sheetDatas[ID].Url);
                }, ID);
            }

            UpdateSlotStatus();

            crossPromoUi.Show();
        }

        public void UnlockItem(int id)
        {
            if (!crossPromoState.availableIdSlots.Contains(id))
            {
                crossPromoState.availableIdSlots.Add(id);
                stateManager.SaveGlobalAsync().Forget();
            }
        }

        public void UnlockRandomItem()
        {
            if (sheetDatas == null && crossPromoState.availableIdSlots.Count == sheetDatas.Length) return;

            List<int> availableIds = Enumerable.Range(0, sheetDatas.Length)
                                               .Except(crossPromoState.availableIdSlots)
                                               .ToList();

            if (availableIds.Count == 0) return;

            int rndId = availableIds[UnityEngine.Random.Range(0, availableIds.Count)];

            crossPromoState.availableIdSlots.Add(rndId);

            stateManager.SaveGlobalAsync().Forget();
        }

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
    }
}
