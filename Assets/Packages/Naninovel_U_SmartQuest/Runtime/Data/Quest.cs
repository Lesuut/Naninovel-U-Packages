using System;
using System.Threading;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public abstract class Quest
    {
        public string Title {  get; set; }

        protected SmartQuestConfiguration smartQuestConfiguration;

        public Quest(string title, SmartQuestConfiguration smartQuestConfiguration)
        {
            Title = title;
            this.smartQuestConfiguration = smartQuestConfiguration;
        }

        public abstract string GetQuestInfo();
        public abstract bool IsQuestComplete();
    }
}
