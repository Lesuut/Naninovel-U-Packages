using UnityEngine;

namespace Naninovel.U.Achievement
{
    [InitializeAtRuntime()]
    public class AchievementService : IAchievementService
    {
        private readonly IStateManager stateManager;
        private AchievementState state;

        public AchievementService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new AchievementState();
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

        private void Serialize(GameStateMap map) => map.SetState(new AchievementState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<AchievementState>();
            state = state == null ? new AchievementState() : new AchievementState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the Achievement Service here
        /// </summary>
    }
}
