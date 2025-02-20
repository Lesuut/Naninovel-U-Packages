using System;
using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    [InitializeAtRuntime()]
    public class CrossPromoService : ICrossPromoService
    {
        public virtual CrossPromoConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private CrossPromoState state;

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

            state = new CrossPromoState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            uiManager = Engine.GetService<IUIManager>();
            unlockableManager = Engine.GetService<IUnlockableManager>();

            sheetDatas = await googleSheetDataLoader.LoadDataAsync(Configuration.GoogleSheetDataURL);
            Debug.Log("CrossPromo: Finish Load GoogleSheetData");
        }

        public void DestroyService()
        {
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() { }

        private void Serialize(GameStateMap map) => map.SetState(new CrossPromoState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<CrossPromoState>();
            state = state == null ? new CrossPromoState() : new CrossPromoState(state);

            return UniTask.CompletedTask;
        }

        public void ShowCrossPromo()
        {
            if (sheetDatas == null || sheetDatas.Length <= 0)
                throw new InvalidOperationException("CrossPromo: sheetDatas not initialized or empty!");

            var crossPromoUi = uiManager.GetUI<CrossPromoUI>();

            crossPromoUi.ClearSlots();

            foreach (var ID in state.availableIdSlots)
            {
                crossPromoUi.SpawnSlot(sheetDatas[ID].Image, () =>
                {
                    crossPromoUi.ShowContinueWindow(() =>
                    {
                        if (!state.viewedIdSlots.Contains(ID))
                        {
                            state.availableIdSlots.Add(ID);
                            unlockableManager.UnlockItem(Configuration.unlockableImages[ID].unlockableKey);
                            stateManager.SaveGlobalAsync().Forget();
                            Debug.Log($"UnlockItem: {Configuration.unlockableImages[ID].unlockableKey}");
                        }

                        crossPromoUi.ShowAdult(Configuration.unlockableImages[ID].sprite);
                    });

                    SteamUrlOpener.OpenUrl(sheetDatas[ID].Url);
                });
            }

            crossPromoUi.Show();
        }

        public void UnlockItem(int id)
        {
            if (!state.availableIdSlots.Contains(id))
                state.availableIdSlots.Add(id);
        }
    }
}
