using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [EditInProjectSettings]
    public class SmartQuestConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "SmartQuest";

        [Header("Title")]
        public string TitleActiveCoding = "%TEXT%";
        public string TitleCompletedCoding = "%TEXT%";
        public Color TitleActiveColor;
        public Color TitleCompletedColor;
        [Header("Options")]
        public string OptionActiveCoding = "• %TEXT%";
        public string OptionCompletedCoding = "• <s>%TEXT%</s>";
        public Color OptionActiveColor;
        public Color OptionCompletedColor;
        [Header("Description")]
        public string DescriptionActiveCoding = "%TEXT%";
        public string DescriptionCompletedCoding = "%TEXT%";
    }
}