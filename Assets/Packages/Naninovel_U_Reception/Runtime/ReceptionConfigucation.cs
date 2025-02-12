using System.Text.RegularExpressions;
using UnityEngine;

namespace Naninovel.U.Reception
{
    /// <summary>
    /// Contains configuration data for the Reception systems.
    /// </summary>
    [EditInProjectSettings]
    public class ReceptionConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "Reception";

        public TextAsset Names;

        public string[] GetNames() => ParseReception(Names);

        private string[] ParseReception(TextAsset textAsset)
        {
            if (textAsset == null) return new string[0];

            // Считываем текст из текстового ассета
            string text = textAsset.text;

            // Создаем регулярное выражение для поиска строк с именами после "Reception."
            Regex regex = new Regex(@"Reception\.(\w+):\s*(\w+)");
            MatchCollection matches = regex.Matches(text);

            // Массив для хранения найденных имён
            string[] names = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                names[i] = matches[i].Groups[1].Value;  // Извлекаем имя (вторую группу)
            }

            return names;
        }
    }
}