using UnityEngine;

namespace Naninovel.U.TestBase
{
    [InitializeAtRuntime()]
    public class TestBaseService : ITestBaseService
    {
        public virtual TestBaseConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private TestBaseState state;

        public TestBaseService(TestBaseConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new TestBaseState();
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

        private void Serialize(GameStateMap map) => map.SetState(new TestBaseState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<TestBaseState>();
            state = state == null ? new TestBaseState() : new TestBaseState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the TestBase service here
        /// </summary>
    }
}
