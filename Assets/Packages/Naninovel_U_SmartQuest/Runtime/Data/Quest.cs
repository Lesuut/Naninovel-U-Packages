using System;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public abstract class Quest
    {
        public string ID;
        public string Title;

        protected SmartQuestConfiguration smartQuestConfiguration;

        public Quest(string id, string title)
        {
            ID = id;
            Title = title;
            smartQuestConfiguration = Engine.GetConfiguration<SmartQuestConfiguration>();
        }

        public abstract string GetQuestInfo();
        public abstract bool IsQuestComplete();
    }
}
