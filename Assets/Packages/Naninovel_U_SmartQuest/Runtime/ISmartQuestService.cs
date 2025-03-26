using System;

namespace Naninovel.U.SmartQuest
{
    public interface ISmartQuestService : IEngineService
    {
        public event Action UpdateQuestAction;
        public string GetQuestsTextInfo();

        public bool GetQuestStatus(string id);

        public void CreateSingleQuest(string id, string title, string description);
        public void CompleteSingleQuest(string id);

        public void CreateMultiQuest(string id, string title, string description);
        public void AddMultiQuestOption(string idQuest, string idOption, int maxProgressUnits, string description);
        public void UpdateInfoAction();
        public void ExecuteMultiQuestOption(string idQuest, string idOption, int value);
        public Quest[] GetAllQuests();
    }
}