using System;
using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public class QuestOption
    {
        public string ID;

        public int ProgressUnits;
        public int MaxProgressUnits;
        public string OptionText;

        private SmartQuestConfiguration smartQuestConfiguration;

        public QuestOption(string id, int maxProgressUnits, string optionText)
        {
            this.ID = id;
            ProgressUnits = 0;
            MaxProgressUnits = Mathf.Clamp(maxProgressUnits, 1, int.MaxValue);
            OptionText = optionText;
            smartQuestConfiguration = Engine.GetConfiguration< SmartQuestConfiguration>();
        }

        public void AddProgressUnit(int progressUnits) => ProgressUnits += progressUnits;

        public string GetOptionText()
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedColor : smartQuestConfiguration.OptionActiveColor)}>" +
                $"{(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedCoding.Replace("%TEXT%", OptionText) : smartQuestConfiguration.OptionActiveCoding.Replace("%TEXT%", OptionText))} " +
                $"{GetProgressText()}</color>";
        }
        public bool IsOptionComplete() => ProgressUnits >= MaxProgressUnits;

        private string GetProgressText()
        {
            if (MaxProgressUnits <= 1)
                return "";

            return $"<color=#{ColorUtility.ToHtmlStringRGBA(smartQuestConfiguration.ProgressGradient.Evaluate((float)ProgressUnits / MaxProgressUnits))}>{ProgressUnits}/{MaxProgressUnits}</color>";
        }
    }
}
