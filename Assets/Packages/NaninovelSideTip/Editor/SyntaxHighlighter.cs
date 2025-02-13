using System;
using System.Text;

public static class SyntaxHighlighter
{
    private const string CommandColor = "#6cb2ed";   // Синий — для @команд (например, @tip, @if)
    private const string KeyColor = "#cd8843";       // Оранжевый — для ключей (часть перед двоеточием, например, key:)
    private const string ValueColor = "#e2be7f";     // Желтый — для значений (текст в кавычках, например, "key_value")
    private const string ArgumentColor = "#dfb3ff";  // Фиолетовый — для аргументов после @ (например, showTip, hideTip)
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

            // Проверяем, содержит ли строка // (начиная с flow-комментария)
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
            sb.Append($"<color={CommandColor}>{words[0]}</color>"); // Красим команду в синий
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
