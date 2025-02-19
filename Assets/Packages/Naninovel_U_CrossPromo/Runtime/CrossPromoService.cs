using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    [InitializeAtRuntime()]
    public class CrossPromoService : ICrossPromoService
    {
        public virtual CrossPromoConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private CrossPromoState state;

        public CrossPromoService(CrossPromoConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new CrossPromoState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            return UniTask.CompletedTask;
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

        /// <summary>
        /// Write the body for the CrossPromo service here
        /// </summary>
    }
}
