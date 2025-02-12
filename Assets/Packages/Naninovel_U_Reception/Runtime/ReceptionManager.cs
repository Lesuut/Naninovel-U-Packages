using UnityEngine;

namespace Naninovel.U.Reception
{
    [InitializeAtRuntime()]
    public class ReceptionManager : IReceptionManager
    {
        public virtual ReceptionConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private ReceptionState state;

        public ReceptionManager(ReceptionConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new ReceptionState();
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

        private void Serialize(GameStateMap map) => map.SetState(new ReceptionState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<ReceptionState>();
            state = state == null ? new ReceptionState() : new ReceptionState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the Reception service here
        /// </summary>
    }
}
