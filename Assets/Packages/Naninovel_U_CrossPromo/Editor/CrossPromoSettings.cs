using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Naninovel.U.CrossPromo
{
    public class CrossPromoSettings : ConfigurationSettings<CrossPromoConfiguration>
    {
        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(CrossPromoConfiguration.API)] = DrawContextKeyItemsEditor;
            return drawers;
        }

        private void DrawContextKeyItemsEditor(SerializedProperty property)
        {
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            var text =
                SyntaxHighlighter.ColorizeSyntax(
                 "\n" +
                 "// Инициализация:\r\n" +
                 "1. Перейдите в папку пакета/Runtime/LeaderBoard и поместите скрипт лидерборда на любой объект. Он является синглтоном. В нем уже прописан метод и хранится стандартный ключ для подсчета количества игроков, которые запустили игру.\r\n" +
                 "2. Возьмите в папке пакета во вкладке UI префаб кросс-промо и добавьте в Naninovel UI.\r\n" +
                 "3. Зайдите в папку DemoTitleButton и поместите префаб кнопки открытия кросс-промо в ваше главное меню.\r\n" +
                 "4. В инспекторе Naninovel укажите ссылку на таблицу (проверьте корректность ссылки, она должна заканчиваться на .csv).\r\n" +
                 "5. Установите статические данные в поля adultStatic для каждого элемента.\r\n" +
                 "6. Установите в поле achievementNaniCommand команду для открытия ачивки при сборе всех слотов.\r\n\n" +
                 "// Эксплуатация:\r\n" +
                 "• Данные о слотах хранятся в NaninovelData/Saves/GlobalSave.\n" +
                 "• Строка в таблице должна иметь такой вид:\n<color=#fff9c7><b>GameLink|ImageLink|LeaderBoardKey</b></color>\n" +
                 "• Заготовьте 9 слотов для наград в инспекторе кросс-промо и в галерее наград. Они будут динамически добавляться и отключаться в зависимости от слотов в таблице.\n" +
                 "• Просмотренные слоты открывают unlockable, которые затем можно отслеживать и использовать в галерее.\r\n" +
                 "• В папке DemoCGSlot лежит пример слота для галереи. Рекомендуется переписать его под свою галерею в зависимости от того, как она у вас устроена.\n" +
                 "• Также добавлены Unity события для различных действий, чтобы можно было подключать частицы и т.д.\r\n\r\n" +
                 "<color=#fff9c7>P.S. В редакторе могут лететь ошибки от LeaderBoardCoroutines, он ругаеться по тому что Steam Manager не инициализирован</color>\n\n" +
                 "@crossPromo //Открыть кросс-промо в конце игры\r\n" +
                 "Записывает в лидерборд как открытие после завершения прохождения\r\n\n" +
                 "Если снять галочку showAllSlotsAtStart, то слоты можно активировать по одному через скрипт\r\n" +
                 "Например, при каждом запуске игры добавляется один слот:\r\n\r" +
                 "@addRndCrossPromo //Добавляет случайный слот\r\n" +
                 "@addCrossPromo 1 //Добавляет слот, указанный по индексу");

            var richTextStyle = new GUIStyle(EditorStyles.textArea) { richText = true, wordWrap = true };
            GUILayout.TextArea(text, richTextStyle);

            EditorGUI.EndProperty();
        }

        private static class SyntaxHighlighter
        {
            private const string CommandColor = "#6cb2ed";   // Синий — для @команд (например, @tip, @if)
            private const string KeyColor = "#cd8843";       // Оранжевый — для ключей (часть перед двоеточием, например, key:)
            private const string ValueColor = "#e2be7f";     // Желтый — для значений (текст в кавычках, например, "key_value")
            private const string ArgumentColor = "#e2be7f";  // Фиолетовый #dfb3ff — для аргументов после @
            private const string FlowCommentColor = "#579f3c"; // Зеленый — для комментариев, начинающихся с //

            public static string ColorizeSyntax(string input)
            {
                var sb = new StringBuilder();
                var lines = input.Split('\n');

                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine))
                    {
                        sb.AppendLine();
                        continue;
                    }

                    int commentIndex = trimmedLine.IndexOf("//", StringComparison.Ordinal);
                    if (commentIndex != -1)
                    {
                        string codePart = trimmedLine.Substring(0, commentIndex);
                        string commentPart = trimmedLine.Substring(commentIndex);

                        sb.Append(ColorizeCodePart(codePart));
                        sb.Append($" <color={FlowCommentColor}>{commentPart}</color>");
                    }
                    else
                    {
                        sb.Append(ColorizeCodePart(trimmedLine));
                    }

                    sb.AppendLine();
                }

                return sb.ToString();
            }

            private static string ColorizeCodePart(string code)
            {
                var sb = new StringBuilder();
                var words = code.Split(' ');

                if (words.Length == 0)
                    return code;

                bool hasAtSymbol = words[0].StartsWith("@");

                if (hasAtSymbol)
                {
                    sb.Append($"<color={CommandColor}>{words[0]}</color>");
                    bool isFirstWordAfterAt = true;

                    for (int i = 1; i < words.Length; i++)
                    {
                        if (words[i].Contains(":"))
                        {
                            var parts = words[i].Split(':');
                            if (parts.Length == 2)
                            {
                                sb.Append($" <color={KeyColor}>{parts[0]}:</color><color={ValueColor}>{parts[1]}</color>");
                            }
                            else
                            {
                                sb.Append($" <color={KeyColor}>{words[i]}</color>");
                            }
                            isFirstWordAfterAt = false;
                        }
                        else if (words[i].StartsWith("\"") && words[i].EndsWith("\""))
                        {
                            sb.Append($" <color={ValueColor}>{words[i]}</color>");
                            isFirstWordAfterAt = false;
                        }
                        else if (words[i] == "-")
                        {
                            sb.Append(" -");
                            sb.Append($" {string.Join(" ", words, i + 1, words.Length - (i + 1))}");
                            break;
                        }
                        else if (isFirstWordAfterAt)
                        {
                            sb.Append($" <color={ArgumentColor}>{words[i]}</color>");
                            isFirstWordAfterAt = false;
                        }
                        else
                        {
                            sb.Append($" {words[i]}");
                        }
                    }
                }
                else
                {
                    sb.Append(code); // Если строка не начинается с @, оставляем белой
                }

                return sb.ToString();
            }
        }
    }
}