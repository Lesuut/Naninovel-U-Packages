using System;
using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public class SingleQuest : Quest
    {
        public string Description;
        public bool Complete;

        public SingleQuest(string title, string description, SmartQuestConfiguration smartQuestConfiguration) : base(title, smartQuestConfiguration)
        {
            Description = description;
            Complete = false;
        }

        public void SetCompleteStatus(bool status) => Complete = status;

        public override string GetQuestInfo()
        {
            return 
                $"<color={ColorUtility.ToHtmlStringRGBA(Complete ? smartQuestConfiguration.OptionCompletedColor : smartQuestConfiguration.TitleActiveColor)}><b>{(Complete ? smartQuestConfiguration.TitleCompletedCoding.Replace("%TEXT%", Title) : smartQuestConfiguration.TitleActiveCoding.Replace("%TEXT%", Title))}</b></color>\n\t" +
                $"<color={ColorUtility.ToHtmlStringRGBA(Complete ? smartQuestConfiguration.OptionCompletedColor : smartQuestConfiguration.OptionActiveColor)}>{(Complete ? smartQuestConfiguration.DescriptionCompletedCoding.Replace("%TEXT%", Description) : smartQuestConfiguration.DescriptionActiveCoding.Replace("%TEXT%", Description))}</color>";
        }

        public override bool IsQuestComplete() => Complete;
    }
}