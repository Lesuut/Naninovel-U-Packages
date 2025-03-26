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
            smartQuestConfiguration = Engine.GetConfiguration<SmartQuestConfiguration>();
        }

        public void AddProgressUnit(int progressUnits) => ProgressUnits += progressUnits;

        public string GetOptionText()
        {
            if (smartQuestConfiguration.useColor)
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedColor : smartQuestConfiguration.OptionActiveColor)}>" +
                       $"{(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedCoding.Replace("%TEXT%", OptionText) : smartQuestConfiguration.OptionActiveCoding.Replace("%TEXT%", OptionText))} " +
                       $"{GetProgressText()}</color>";
            }
            else
            {
                return $"{(ProgressUnits >= MaxProgressUnits ? smartQuestConfiguration.OptionCompletedCoding.Replace("%TEXT%", OptionText) : smartQuestConfiguration.OptionActiveCoding.Replace("%TEXT%", OptionText))} " +
                       $"{GetProgressText(false)}";
            }
        }

        public bool IsOptionComplete() => ProgressUnits >= MaxProgressUnits;

        private string GetProgressText(bool useColor = true)
        {
            if (MaxProgressUnits <= 1)
                return "";

            string progressText = $"{ProgressUnits}/{MaxProgressUnits}";

            if (useColor && smartQuestConfiguration.useColor)
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(smartQuestConfiguration.ProgressGradient.Evaluate((float)ProgressUnits / MaxProgressUnits))}>{progressText}</color>";
            }

            return progressText;
        }
    }
}
