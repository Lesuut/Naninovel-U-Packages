namespace Naninovel.U.Base
{
    [InitializeAtRuntime()]
    public class BaseServiceA : IBaseAService
    {
        private readonly IStateManager stateManager;
        private BaseState state;

        public BaseServiceA(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new BaseState();
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

        private void Serialize(GameStateMap map) => map.SetState(new BaseState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<BaseState>();
            state = state == null ? new BaseState() : new BaseState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the Base service here
        /// </summary>
    }
}