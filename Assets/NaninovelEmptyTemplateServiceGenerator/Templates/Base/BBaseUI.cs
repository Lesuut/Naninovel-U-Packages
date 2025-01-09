using Naninovel.UI;
using System;
using System.Collections.Generic;

namespace Naninovel.U.Base
{
    public class BBaseUI : CustomUI
    {
        [Serializable]
        private new class GameState
        {
            public string Value;
        }

        private IBaseBService baseService;

        protected override void Awake()
        {
            base.Awake();

            baseService = Engine.GetService<IBaseBService>();
        }

        /// <summary>
        /// Write the body for the Base UI here
        /// </summary>

        protected override void SerializeState(GameStateMap stateMap)
        {
            // Invoked when the game is saved.

            base.SerializeState(stateMap);

            // Serialize UI state.
            var state = new GameState
            {
                Value = "Value1"
            };
            stateMap.SetState(state);
        }

        protected override async UniTask DeserializeState(GameStateMap stateMap)
        {
            // Invoked when the game is loaded.

            await base.DeserializeState(stateMap);

            var state = stateMap.GetState<GameState>();
            if (state is null) return; // empty state, do nothing

            // Set data state here

            UnityEngine.Debug.Log($"Exemple: {state.Value}!");
        }
    }
}
