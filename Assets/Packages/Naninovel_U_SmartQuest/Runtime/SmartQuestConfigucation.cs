using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [EditInProjectSettings]
    public class SmartQuestConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "SmartQuest";

        [Header("Title")]
        public string TitleActiveCoding = "<b>%TEXT%</b>";
        public string TitleCompletedCoding = "<b>%TEXT%</b> +";
        public Color TitleActiveColor = new Color(1, 1, 1f, 1);
        public Color TitleCompletedColor = new Color(0.5f, 0.5f, 0.5f, 1);
        [Header("Options")]
        public string OptionActiveCoding = "• %TEXT%";
        public string OptionCompletedCoding = "• <s>%TEXT%</s>";
        public Color OptionActiveColor = new Color(1, 1, 1f, 1);
        public Color OptionCompletedColor = new Color(0.5f, 0.5f, 0.5f, 1);
        [Tooltip("Color of progress, like 2/5")]
        public Gradient ProgressGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color(1f, 0f, 0f, 1f), 0f), // Красный (Color.red)
                new GradientColorKey(new Color(1f, 1f, 0f, 1f), 0.5f), // Желтый (Color.yellow)
                new GradientColorKey(new Color(0f, 1f, 0f, 1f), 0.9f), // Зеленый (Color.green)
                new GradientColorKey(new Color(0.5f, 0.5f, 0.5f, 1), 1f) 
            }
        };
        [Header("Description")]
        public string DescriptionActiveCoding = "%TEXT%";
        public string DescriptionCompletedCoding = "%TEXT%";
        public Color DescriptionActiveColor = new Color(1, 1, 1f, 1);
        public Color DescriptionCompletedColor = new Color(0.5f, 0.5f, 0.5f, 1);
    }
}