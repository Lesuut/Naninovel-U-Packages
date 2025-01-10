namespace Naninovel.U.MyOne
{
    [InitializeAtRuntime()]
    public class MyOneService : IMyOneService
    {
        private readonly IStateManager stateManager;
        private MyOneState state;

        public MyOneService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new MyOneState();
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

        private void Serialize(GameStateMap map) => map.SetState(new MyOneState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<MyOneState>();
            state = state == null ? new MyOneState() : new MyOneState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the MyOne Service here
        /// </summary>
    }
}
