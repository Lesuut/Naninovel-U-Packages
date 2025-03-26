using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public class MultiQuest : SingleQuest
    {
        public List<QuestOption> Options;

        public MultiQuest(string id, string title, string description) : base(id, title, description)
        {
            Options = new List<QuestOption>();
        }

        public override string GetQuestInfo()
        {
            string options = string.Join("\n", Options.Select(item => $"\t{item.GetOptionText()}")) + "\n";

            string description;

            if (smartQuestConfiguration.useColor)
            {
                description = $"<color=#{ColorUtility.ToHtmlStringRGBA(IsQuestComplete() ? smartQuestConfiguration.DescriptionCompletedColor : smartQuestConfiguration.DescriptionActiveColor)}>" +
                              $"{(IsQuestComplete() ? smartQuestConfiguration.DescriptionCompletedCoding.Replace("%TEXT%", Description) : smartQuestConfiguration.DescriptionActiveCoding.Replace("%TEXT%", Description))}" +
                              $"</color>";
            }
            else
            {
                description = IsQuestComplete()
                    ? smartQuestConfiguration.DescriptionCompletedCoding.Replace("%TEXT%", Description)
                    : smartQuestConfiguration.DescriptionActiveCoding.Replace("%TEXT%", Description);
            }

            return $"{options}{(string.IsNullOrEmpty(Description) ? "" : $"\n{description}")}";
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
