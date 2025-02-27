using DG.Tweening;
using UnityEngine;

namespace Naninovel.U.ChoiceBox
{
    [InitializeAtRuntime()]
    public class ChoiceBoxService : IChoiceBoxService
    {
        private readonly IStateManager stateManager;
        private IScriptPlayer scriptPlayer;
        private IUIManager uiManager;

        private ChoiceBoxState state;

        public ChoiceBoxService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new ChoiceBoxState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);
            scriptPlayer = Engine.GetService<IScriptPlayer>();
            uiManager = Engine.GetService<IUIManager>();

            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() 
        {
            uiManager.GetUI<ChoiceBoxUI>().DestroyAllChoices();
            state.isChoiceBoxActive = false;
            state.choiceItem.Clear();
        }

        private void Serialize(GameStateMap map) => map.SetState(new ChoiceBoxState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<ChoiceBoxState>();
            state = state == null ? new ChoiceBoxState() : new ChoiceBoxState(state);

            if (state.isChoiceBoxActive)
            {
                SetTitle(state.currentChoiceTitle);

                foreach (var item in state.choiceItem)
                {
                    AddOption(item.Title, item.Title);
                }

                ShowChoiceBox();
            }

            return UniTask.CompletedTask;
        }

        public void AddOption(string title, string toDo)
        {
            /*Debug.Log($"AddOption: {title} todo:{toDo}");*/

            ChoiceBoxUI choiceBoxUI = uiManager.GetUI<ChoiceBoxUI>();
            choiceBoxUI.CreateChoice(title, () => PlayScript(toDo));
            state.choiceItem.Add(new ChoiceBoxItem() { Title = title, ToDo = toDo});;
        }

        public void SetTitle(string title)
        {
            ChoiceBoxUI choiceBoxUI = uiManager.GetUI<ChoiceBoxUI>();
            choiceBoxUI.SetTitle(title);
        }

        public void ShowChoiceBox()
        {
            if (state.choiceItem.Count <= 0) return;

            uiManager.GetUI<ChoiceBoxUI>().Show();

            state.startScriptPlayedIndex = scriptPlayer.PlayedIndex;
            scriptPlayer.Stop();
            state.isChoiceBoxActive = true;
        }
        private async void PlayScript(string scriptText)
        {
            var script = Script.FromScriptText($"Generated script", scriptText);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();

            ChoiceBoxUI choiceBoxUI = uiManager.GetUI<ChoiceBoxUI>();
            choiceBoxUI.Hide();

            choiceBoxUI.ClearChoices();
            state.choiceItem.Clear();
            state.isChoiceBoxActive = false;

            scriptPlayer.Play(scriptPlayer.Playlist, state.startScriptPlayedIndex + 1);
        }
    }
}
