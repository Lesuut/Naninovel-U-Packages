using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    [InitializeAtRuntime()]
    public class HotelManagementManager : IHotelManagementManager
    {
        public virtual HotelManagementConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private HotelManagementState state;

        public HotelManagementManager(HotelManagementConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new HotelManagementState();
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

        private void Serialize(GameStateMap map) => map.SetState(new HotelManagementState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<HotelManagementState>();
            state = state == null ? new HotelManagementState() : new HotelManagementState(state);

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Write the body for the HotelManagement service here
        /// </summary>
    }
}
