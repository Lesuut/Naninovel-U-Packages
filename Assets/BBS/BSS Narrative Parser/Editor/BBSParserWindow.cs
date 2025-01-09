using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.BBSParserNarrative
{
    public class BBSParserWindow : EditorWindow
    {
        private string inputCsvPath;
        private string outputTextPath;

        private string fileName = "New Nani Narrative";

        private int linesToParse = 2000;

        private string originReplaseNameMainChar;
        private string replaseNameMainChar = "{MainCharacter}";

        private int minNameRate = 5;

        private string extention = "txt";
        private char splitChar = ';';

        [SerializeField]
        private List<CharacterNameWeightItem> characterNames = new List<CharacterNameWeightItem>();
        private SerializedObject serializedObject;
        private SerializedProperty characterListProperty;

        private const string InputCsvPathKey = "BBSParserInputCsvPath";
        private const string OutputTextPathKey = "BBSParserOutputTextPath";
        private const string CharacterNamesKey = "BBSParserCharacterNames";
        private const string OriginReplaceNameKey = "BBSParserOriginReplaceName";
        private const string ReplaceNameMainCharKey = "BBSParserReplaceNameMainChar";
        private const string LinesToParseKey = "BBSParserLinesToParse";

        [System.Serializable]
        private class CharacterNameWeightItem
        {
            public string Name;
            public int Count = 0;
            [Header("Settings")]
            public string ReplaceName = "";
            public bool mark = false;
        }

        [System.Serializable]
        private class CharacterNamesListWrapper
        {
            public List<CharacterNameWeightItem> characterNames;
        }

        [MenuItem("Tools/CSV to Text Converter")]
        public static void OpenWindow()
        {
            GetWindow<BBSParserWindow>("CSV to Text Converter");
        }

        private void OnEnable()
        {
            inputCsvPath = EditorPrefs.GetString(InputCsvPathKey, string.Empty);
            outputTextPath = EditorPrefs.GetString(OutputTextPathKey, string.Empty);

            linesToParse = EditorPrefs.GetInt(LinesToParseKey, 1000);

            originReplaseNameMainChar = EditorPrefs.GetString(OriginReplaceNameKey, string.Empty);
            replaseNameMainChar = EditorPrefs.GetString(ReplaceNameMainCharKey, "{MainCharacter}");

            LoadCharacterNames();

            serializedObject = new SerializedObject(this);
            characterListProperty = serializedObject.FindProperty("characterNames");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("BBS Parser Narrative", EditorStyles.boldLabel);

            fileName = EditorGUILayout.TextField("Output File Name", fileName);

            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                inputCsvPath = EditorGUILayout.TextField("Input CSV Path", inputCsvPath);
                if (GUILayout.Button("Select", GUILayout.Width(65)))
                {
                    inputCsvPath = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
                    if (!string.IsNullOrEmpty(inputCsvPath))
                    {
                        EditorPrefs.SetString(InputCsvPathKey, inputCsvPath);
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                outputTextPath = EditorGUILayout.TextField("Output Text Path", outputTextPath);
                if (GUILayout.Button("Select", GUILayout.Width(65)))
                {
                    outputTextPath = EditorUtility.SaveFilePanel("Save Text File", "", fileName, extention);
                    if (!string.IsNullOrEmpty(outputTextPath))
                    {
                        EditorPrefs.SetString(OutputTextPathKey, outputTextPath);
                    }
                }
            }

            linesToParse = EditorGUILayout.IntField("Lines To Parse", linesToParse);

            EditorGUILayout.Space();

            originReplaseNameMainChar = EditorGUILayout.TextField("Origin Name MainChar", originReplaseNameMainChar);
            replaseNameMainChar = EditorGUILayout.TextField("Replase Name Main Char", replaseNameMainChar);

            EditorGUILayout.Space();

            minNameRate = EditorGUILayout.IntField("Min Name Rate Count", minNameRate);

            if (GUILayout.Button("Find All Names"))
            {
                if (EditorUtility.DisplayDialog(
                    "Confirmation",
                    "Are you sure you want to find all names? This operation cannot be undone.",
                    "Yes",
                    "No"))
                {
                    characterNames = GetCharactersName(LoadCsvData(inputCsvPath), minNameRate);
                    UpdateCharacterListInSerializedProperty(); // Обновляем список через SerializedProperty
                    SaveCharacterNames(); // Сохраняем список персонажей
                }
            }

            serializedObject.Update(); // Обновляем состояние объекта
            EditorGUILayout.PropertyField(characterListProperty, true);
            serializedObject.ApplyModifiedProperties(); // Применяем изменения
            SaveCharacterNames();
            EditorPrefs.SetString(OriginReplaceNameKey, originReplaseNameMainChar);
            EditorPrefs.SetString(ReplaceNameMainCharKey, replaseNameMainChar);
            EditorPrefs.SetInt(LinesToParseKey, linesToParse);

            // Кнопка для конвертации
            if (GUILayout.Button("Convert"))
            {
                if (string.IsNullOrEmpty(inputCsvPath) || string.IsNullOrEmpty(outputTextPath))
                {
                    EditorUtility.DisplayDialog("Error", "Please specify both input CSV path and output text path.", "OK");
                    return;
                }

                ConvertCsvToText(inputCsvPath, outputTextPath);
                EditorUtility.DisplayDialog("Success", "CSV file successfully converted to text.", "OK");
            }
        }

        private void UpdateCharacterListInSerializedProperty()
        {
            characterListProperty.ClearArray(); // Очищаем список в SerializedProperty
            foreach (var character in characterNames)
            {
                characterListProperty.InsertArrayElementAtIndex(characterListProperty.arraySize);
                var element = characterListProperty.GetArrayElementAtIndex(characterListProperty.arraySize - 1);
                element.FindPropertyRelative("Name").stringValue = character.Name;
                element.FindPropertyRelative("Count").intValue = character.Count;
            }
        }

        // Новый метод для загрузки данных из CSV
        private List<string[]> LoadCsvData(string csvPath)
        {
            if (!File.Exists(csvPath))
                throw new FileNotFoundException("CSV file not found.", csvPath);

            var lines = File.ReadAllLines(csvPath);
            var matrixContent = new List<string[]>();

            // Разделение каждой строки CSV на элементы и сохранение в двумерный массив
            foreach (var line in lines)
            {
                var elements = line.Split(splitChar);
                matrixContent.Add(elements); // Добавляем массив строк в матрицу
            }

            return matrixContent;
        }

        private void ConvertCsvToText(string csvPath, string textPath)
        {
            // Загружаем данные из CSV
            var matrixContent = LoadCsvData(csvPath);

            if (File.Exists(textPath))
            {
                if (EditorUtility.DisplayDialog("File Already Exists",
                    $"The file \"{textPath}\" already exists. Do you want to overwrite it?",
                    "Yes", "No"))
                {
                    var processedText = MagicNaniParser(matrixContent);
                    File.WriteAllLines(textPath, processedText);
                }
                else
                {
                    Debug.Log("File not overwritten.");
                }
            }
            else
            {
                var processedText = MagicNaniParser(matrixContent);
                File.WriteAllLines(textPath, processedText);
            }
        }

        private List<string> TextDebugSeparator(List<string[]> matrixContent)
        {
            var processedText = new List<string>();

            // Проходим по строкам матрицы
            foreach (var row in matrixContent)
            {
                // Каждый элемент строки соединяется через запятую, и строка заключена в квадратные скобки
                processedText.Add($"{string.Join("____+____", row)}");
            }

            return processedText;
        }

        private List<string> MagicNaniParser(List<string[]> matrixContent)
        {
            var processedText = new List<string>();

            string lastCharacterName = "";
            int currentChoiseOrder = 0;

            for (int i = 0; i < matrixContent.Count; i++)
            {
                if (matrixContent[i].Length < 9) continue;

                string newLine = "";

                if (matrixContent[i][0].ToLower().Contains("выбор".ToLower()))
                {

                    newLine = $"\n;-----------------------------------------\n;{matrixContent[i][0]} LINE: {i}";
                    processedText.Add(newLine);
                    currentChoiseOrder = 0;
                    continue;
                }
                if (ContainsAnyChoiceSuffix(matrixContent[i][1]))
                {
                    // если там есть и это не имя тогда в ручную записуем что там

                    if (!string.IsNullOrEmpty(matrixContent[i][0]))
                    {
                        var foundCharacter = characterNames.FirstOrDefault(charItem => charItem.Name == matrixContent[i][0].Replace(":", "").Replace("\n", "").Replace("\r", "").Trim());

                        if (foundCharacter == null)
                        {
                            newLine = $"{matrixContent[i][1]} ;{matrixContent[i][0]}";
                        }
                        else
                        {
                            if (foundCharacter != null)
                            {
                                lastCharacterName = foundCharacter.Name.Trim();
                                newLine = $"{(foundCharacter.mark ? $";Origin: {foundCharacter.Name}\n" : "")}{(matrixContent[i][1].Contains("\"") ? "Thoughts" : "")}{(!string.IsNullOrEmpty(foundCharacter.ReplaceName) ? foundCharacter.ReplaceName : foundCharacter.Name)}: {matrixContent[i][1].Replace("\"", "")}";
                            }
                            else if (foundCharacter == null && !string.IsNullOrEmpty(lastCharacterName))
                            {
                                foundCharacter = characterNames.FirstOrDefault(charItem => charItem.Name == lastCharacterName);
                                newLine = $"{(foundCharacter.mark ? $";Origin: {foundCharacter.Name}\n" : "")}{(matrixContent[i][1].Contains("\"") ? "Thoughts" : "")}{(!string.IsNullOrEmpty(foundCharacter.ReplaceName) ? foundCharacter.ReplaceName : foundCharacter.Name)}: {matrixContent[i][1].Replace("\"", "")}";
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        newLine = $"{matrixContent[i][1]}";
                    }

                    int lineValue = GetLargestNumberFromText(matrixContent[i][1]);

                    if (currentChoiseOrder < lineValue)
                    {
                        currentChoiseOrder = lineValue;
                        newLine = $"@choice \"{newLine}\" goto:.";
                    }
                    else
                    {
                        if (currentChoiseOrder != lineValue && lineValue == 1)
                        {
                            newLine = ";-----------------------------------------\n" + newLine;
                        }
                    }

                    //processedText.Add(newLine.Replace(originReplaseNameMainChar, replaseNameMainChar));
                    processedText.Add(ProcessText(newLine, originReplaseNameMainChar, replaseNameMainChar));

                    continue;
                }

                if (string.IsNullOrEmpty(matrixContent[i][1]) && string.IsNullOrEmpty(matrixContent[i][8]) && string.IsNullOrEmpty(matrixContent[i][7])) continue;

                if (!string.IsNullOrEmpty(matrixContent[i][1]))
                {
                    var foundCharacter = characterNames.FirstOrDefault(charItem => charItem.Name == matrixContent[i][0].Replace(":", "").Replace("\n", "").Replace("\r", "").Trim());

                    if (foundCharacter != null)
                    {
                        lastCharacterName = foundCharacter.Name.Trim();
                        newLine = $"{(foundCharacter.mark ? $";Origin: {foundCharacter.Name}\n" : "")}{(matrixContent[i][1].Contains("\"") ? "Thoughts" : "")}{(!string.IsNullOrEmpty(foundCharacter.ReplaceName) ? foundCharacter.ReplaceName : foundCharacter.Name)}: {matrixContent[i][1].Replace("\"", "")}";
                    }
                    else if (foundCharacter == null && !string.IsNullOrEmpty(lastCharacterName))
                    {
                        foundCharacter = characterNames.FirstOrDefault(charItem => charItem.Name == lastCharacterName);
                        newLine = $"{(foundCharacter.mark ? $";Origin: {foundCharacter.Name}\n" : "")}{(matrixContent[i][1].Contains("\"") ? "Thoughts" : "")}{(!string.IsNullOrEmpty(foundCharacter.ReplaceName) ? foundCharacter.ReplaceName : foundCharacter.Name)}: {matrixContent[i][1].Replace("\"", "")}";
                    }
                    else
                    {
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(matrixContent[i][8]))
                {
                    newLine = $"@back {matrixContent[i][8]} id:Null{(string.IsNullOrEmpty(newLine) ? "" : "\n")}{newLine}";
                }

                if (!string.IsNullOrEmpty(matrixContent[i][7]))
                {
                    newLine = $"@sfx {matrixContent[i][7]}{(string.IsNullOrEmpty(newLine) ? "" : "\n")}{newLine}";
                }

                if (i > linesToParse) break;

                //processedText.Add(newLine.Replace(originReplaseNameMainChar, replaseNameMainChar));
                processedText.Add(ProcessText(newLine, originReplaseNameMainChar, replaseNameMainChar));
            }

            return processedText;
        }

        private bool ContainsAnyChoiceSuffix(string input)
        {
            var suffixes = new[] { "1.", "2.", "3.", "4.", "5.", "6.", "7.", "8.", "9.", "10." };
            return suffixes.Any(suffix => input.Contains(suffix));
        }

        private int GetLargestNumberFromText(string input)
        {
            var matches = System.Text.RegularExpressions.Regex.Matches(input, @"\d+");

            if (matches.Count > 0)
            {
                var numbers = matches.Cast<System.Text.RegularExpressions.Match>()
                                     .Select(m => int.Parse(m.Value))
                                     .ToArray();

                return numbers.Max();
            }

            return 0;
        }

        public static bool ContainsDigit(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                    return true;
            }

            return false;
        }

        private List<CharacterNameWeightItem> GetCharactersName(List<string[]> matrixContent, int minRepetitionRate)
        {
            var characters = matrixContent
                .Where(lineItem => !string.IsNullOrEmpty(lineItem[0]) && lineItem[0][0] != '"' && (lineItem.Length > 1 ? !string.IsNullOrEmpty(lineItem[1]) : false)) // Отбираем только те строки, где имя персонажа не пустое
                .Select(lineItem => lineItem[0]
                    .Trim() // Убираем пробелы с обеих сторон
                    .Replace(":", "") // Убираем двоеточие
                    .Replace("\n", "") // Убираем символы переноса строки
                    .Replace("\r", "") // Убираем символы возврата каретки
                ) // Преобразуем строку
                .Where(name => !string.IsNullOrEmpty(name)) // Убираем пустые строки
                .GroupBy(name => name) // Группируем по имени персонажа
                .Select(group => new CharacterNameWeightItem
                {
                    Name = group.Key,
                    Count = group.Count() // Считаем количество повторений
                })
                .Where(character => character.Count > minRepetitionRate) // Отбираем те, у которых количество повторений больше minRepetitionRate
                .OrderByDescending(character => character.Count) // Сортируем по убыванию количества
                .ToList(); // Преобразуем в список

            return characters;
        }

        private void SaveCharacterNames()
        {
            var wrapper = new CharacterNamesListWrapper { characterNames = characterNames };
            var json = JsonUtility.ToJson(wrapper);
            EditorPrefs.SetString(CharacterNamesKey, json);
        }

        private void LoadCharacterNames()
        {
            if (EditorPrefs.HasKey(CharacterNamesKey))
            {
                var json = EditorPrefs.GetString(CharacterNamesKey);
                var wrapper = JsonUtility.FromJson<CharacterNamesListWrapper>(json);
                if (wrapper != null && wrapper.characterNames != null)
                {
                    characterNames = wrapper.characterNames;
                }
            }
        }
        public string ProcessText(string inputText, string originReplaceNameMainChar, string replaceNameMainChar)
        {
            // Разделяем строку на элементы по запятой и убираем пробелы
            var elements = replaceNameMainChar.Split(',');
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = elements[i].Trim(); // Удаляем пробелы
            }

            // Выполняем замену для каждого элемента и собираем результат в одну строку
            string result = string.Empty;
            foreach (var element in elements)
            {
                result += inputText.Replace(originReplaceNameMainChar, element) + Environment.NewLine;
            }

            return result.Trim(); // Убираем лишний перевод строки в конце
        }
    }
}
