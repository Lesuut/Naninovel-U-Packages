using System;
using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public class QuestOption
    {
        public int ProgressUnits;
        public int MaxProgressUnits;
        public string OptionText;

        private SmartQuestConfiguration smartQuestConfiguration;

        public QuestOption(int maxProgressUnits, string optionText, SmartQuestConfiguration smartQuestConfiguration)
        {
            ProgressUnits = 0;
            MaxProgressUnits = maxProgressUnits;
            OptionText = optionText;
            this.smartQuestConfiguration = smartQuestConfiguration;
        }

        public void AddProgressUnit(int progressUnits) => ProgressUnits += progressUnits;

        public string GetOptionText()
        {
            return $"<color={ColorUtility.ToHtmlStringRGBA(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedColor : smartQuestConfiguration.OptionActiveColor)}>" +
                $"{(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedCoding.Replace("%TEXT%", OptionText) : smartQuestConfiguration.OptionActiveCoding.Replace("%TEXT%", OptionText))}</color>";
        }
        public bool IsOptionComplete() => ProgressUnits >= MaxProgressUnits;
    }
}
