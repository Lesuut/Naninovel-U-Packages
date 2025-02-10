using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [InitializeAtRuntime()]
    public class SmartQuestService : ISmartQuestService
    {
        public virtual SmartQuestConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private SmartQuestState state;

        public SmartQuestService(SmartQuestConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new SmartQuestState();
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

        private void Serialize(GameStateMap map) => map.SetState(new SmartQuestState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<SmartQuestState>();
            state = state == null ? new SmartQuestState() : new SmartQuestState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the SmartQuest service here
        /// </summary>
    }
}
