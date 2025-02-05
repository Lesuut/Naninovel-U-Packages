using UnityEngine;

namespace Naninovel.U.ParametersChoice
{
    [InitializeAtRuntime()]
    public class ParametersChoiceManager : IParametersChoiceManager
    {
        public virtual ParametersChoiceConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private ParametersChoiceState state;

        public ParametersChoiceManager(ParametersChoiceConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new ParametersChoiceState();
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

        private void Serialize(GameStateMap map) => map.SetState(new ParametersChoiceState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<ParametersChoiceState>();
            state = state == null ? new ParametersChoiceState() : new ParametersChoiceState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the ParametersChoice service here
        /// </summary>
    }
}
