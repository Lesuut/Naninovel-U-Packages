using System.Linq;

namespace Naninovel.U.Parameters
{
    [InitializeAtRuntime()]
    public class ParametersManager : IParametersManager
    {
        private readonly IStateManager stateManager;
        private ParametersState state;

        public ParametersManager(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new ParametersState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() 
        { 
            state.ResetAllParameters();
        }

        private void Serialize(GameStateMap map) => map.SetState(new ParametersState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<ParametersState>();
            state = state == null ? new ParametersState() : new ParametersState(state);

            return UniTask.CompletedTask;
        }

        public void SetParametrOperation(string key, int value)
        {
            state.Parameters.FirstOrDefault(item => item.Key == key).Value += value;
        }

        public float GetParametrOperation(string key)
        {
            return state.Parameters.FirstOrDefault(item => item.Key == key).Value;
        }
    }
}
