using Naninovel.UI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Naninovel.U.SideTip
{
    public class TipUI : CustomUI
    {
        [Serializable]
        private class GameState
        {
            public string Key;
        }

        [Space(10)]
        [SerializeField] private UnityEvent<string> onValueSet;
        [Space]
        [SerializeField] private Animation anim;
        [SerializeField] private AnimationClip animationClipShow;
        [SerializeField] private AnimationClip animationClipHide;

        private TipManager tipManager;
        private ITextManager textManager;

        public string CurrentKey { get; private set; } = "";

        public void ShowTip(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    Debug.LogError("ShowTip key is null or empty!");
                    return;
                }

                tipManager.Configuration.ContextKeyItems.First(item => item.Key == key);
            }
            catch (InvalidOperationException ex)
            {
                Debug.LogError($"Element with key '{key}' is not found in the Tip configuration!\n{ex.Message}");
                throw ex;
            }

            string value = textManager.GetRecordValue(key, tipManager.Configuration.Category);

            if (string.IsNullOrEmpty(value))
            {
                value = tipManager.Configuration.ContextKeyItems.FirstOrDefault(item => item.Key == key).Value;
                Debug.LogWarning($"The value: '{value}' for Tip was obtained from the configuration. For correct localization generate managed text documents.");
            }

            if (CurrentKey == "" && (anim != null && animationClipShow != null))
            {
                anim.clip = animationClipShow;
                anim.Play();
            }

            CurrentKey = key;
            onValueSet?.Invoke(value);
        }
        public void HideTip()
        {
            if (anim != null && animationClipShow != null)
            {
                anim.clip = animationClipHide;
                anim.Play();
            }

            CurrentKey = "";
        }

        protected override void Awake()
        {
            base.Awake();

            tipManager = Engine.GetService<TipManager>();
            textManager = Engine.GetService<ITextManager>();
        }

        protected override void SerializeState(GameStateMap stateMap)
        {
            // Invoked when the game is saved.
            base.SerializeState(stateMap);

            // Serialize UI state.
            var state = new GameState
            {
                Key = CurrentKey,
            };

            stateMap.SetState(state);
        }

        protected override async UniTask DeserializeState(GameStateMap stateMap)
        {
            // Invoked when the game is loaded.
            await base.DeserializeState(stateMap);

            var state = stateMap.GetState<GameState>();

            if (state is null) return; // empty state, do nothing

            CurrentKey = state.Key;

            if (!string.IsNullOrEmpty(CurrentKey))
            {
                ShowTip(state.Key);
            }
        }
    }
}