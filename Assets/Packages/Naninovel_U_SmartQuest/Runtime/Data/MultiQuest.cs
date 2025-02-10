using System.Collections.Generic;

namespace Naninovel.U.SmartQuest
{
    public class MultiQuest : Quest
    {
        public List<QuestOption> Options;

        public MultiQuest(string title, SmartQuestConfiguration smartQuestConfiguration) : base(title, smartQuestConfiguration)
        {
            Options = new List<QuestOption>();
        }

        public override string GetQuestInfo()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsQuestComplete()
        {
            foreach (var item in Options)
            {
                if (!item.IsOptionComplete())
                    return false;
            }
            return true;
        }
    }
}
