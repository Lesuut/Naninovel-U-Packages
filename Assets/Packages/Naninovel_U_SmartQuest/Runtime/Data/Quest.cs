using System;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public abstract class Quest
    {
        public string ID { get; set; }
        public string Title { get; set; }

        protected SmartQuestConfiguration smartQuestConfiguration;

        public Quest(string id, string title)
        {
            ID = id;
            Title = title;
            smartQuestConfiguration = Engine.GetConfiguration<SmartQuestConfiguration>();
        }

        public abstract string GetQuestTitle();
        public abstract string GetQuestInfo();
        public abstract bool IsQuestComplete();
    }
}
