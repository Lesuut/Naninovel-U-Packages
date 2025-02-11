using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    [InitializeAtRuntime()]
    public class UIInteractionToolkitManager : IUIInteractionToolkitManager
    {
        public virtual UIInteractionToolkitConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private UIInteractionToolkitState state;

        public UIInteractionToolkitManager(UIInteractionToolkitConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new UIInteractionToolkitState();
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

        private void Serialize(GameStateMap map) => map.SetState(new UIInteractionToolkitState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<UIInteractionToolkitState>();
            state = state == null ? new UIInteractionToolkitState() : new UIInteractionToolkitState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the UIInteractionToolkit service here
        /// </summary>
    }
}
