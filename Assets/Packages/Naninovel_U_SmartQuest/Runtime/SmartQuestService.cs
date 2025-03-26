using System;
using System.Linq;
using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [InitializeAtRuntime()]
    public class SmartQuestService : ISmartQuestService
    {
        public virtual SmartQuestConfiguration Configuration { get; }

        public event Action UpdateQuestAction;

        private readonly IStateManager stateManager;
        private SmartQuestState state;

        public SmartQuestService(SmartQuestConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new SmartQuestState();
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
            state.Quests.Clear();
        }

        private void Serialize(GameStateMap map) 
        {
            state.SaveData();
            map.SetState(new SmartQuestState(state));
        }

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<SmartQuestState>();
            state = state == null ? new SmartQuestState() : new SmartQuestState(state);

            state.LoadData();

            UpdateInfoAction();

            return UniTask.CompletedTask;
        }

        public bool GetQuestStatus(string id)
        {
            return state.Quests.FirstOrDefault(item => item.ID == id).IsQuestComplete();
        }

        public void CreateSingleQuest(string id, string title, string description)
        {
            state.Quests.Add(new SingleQuest(id, title, description));
            UpdateInfoAction();
        }

        public void CompleteSingleQuest(string id)
        {
            foreach (var item in state.Quests)
            {
                if (item.ID == id)
                {
                    if (item is SingleQuest)
                    {
                        ((SingleQuest)item).SetCompleteStatus(true);
                        UpdateInfoAction();
                        return;
                    }
                }
            }

            Debug.LogError($"SmartQuestService: CompleteSingleQuest with ID:{id} not found!");
        }

        public void CreateMultiQuest(string id, string title, string description)
        {
            state.Quests.Add(new MultiQuest(id, title, description));
        }

        public void AddMultiQuestOption(string idQuest, string idOption, int maxProgressUnits, string description)
        {
            Quest quest = state.Quests.FirstOrDefault(item => item.ID == idQuest);
            ((MultiQuest)quest).Options.Add(new QuestOption(idOption, maxProgressUnits == 0 ? 1 : maxProgressUnits, description));
        }

        public void UpdateInfoAction()
        {
            UpdateQuestAction?.Invoke();
        }

        public void ExecuteMultiQuestOption(string idQuest, string idOption, int value)
        {
            Quest quest = state.Quests.FirstOrDefault(item => item.ID == idQuest);
            ((MultiQuest)quest).Options.FirstOrDefault(item => item.ID == idOption).AddProgressUnit(value == 0 ? 1 : value);
            UpdateInfoAction();
        }
        public string GetQuestsTextInfo()
        {
            var sortedQuests = state.Quests
                .OrderBy(q => !q.IsQuestComplete())
                .Reverse()
                .Select(q => 
                $"{q.GetQuestTitle()}" +
                $"{(string.IsNullOrWhiteSpace(q.GetQuestInfo()) ? "" : "\n" + q.GetQuestInfo())}");

            return string.Join("\n\n", sortedQuests);
        }

        public Quest[] GetAllQuests()
        {
            if (state.Quests == null || !state.Quests.Any())
                return Array.Empty<Quest>();

            var sortedQuests = state.Quests
                .OrderBy(q => !q.IsQuestComplete())
                .Reverse();

            return sortedQuests.ToArray();
        }
    }
}
