using Codice.Client.BaseCommands.Changelist;
using Naninovel.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.BSSNaniBugNamesReplaseParser
{
    public class BSSNaniBugNamesReplaseParser : EditorWindow
    {
        private string inputScriptPath = "";
        private string inputNarrativePath = "";
        private string outputScriptPath = "";

        public string[] names = Array.Empty<string>();

        private SerializedObject serializedObject;
        private SerializedProperty namesProperty;

        private int replaceCounter = 0;
        private string replaseLineList = "";

        private const string InputScriptPathKey = "InputScriptPath";
        private const string InputNarrativePathKey = "InputNarrativePath";
        private const string OutputScriptPathKey = "OutputScriptPath";
        private const string NamesKey = "Names";

        [MenuItem("Tools/Nani Bug Names Replace Parser")]
        public static void ShowWindow()
        {
            GetWindow<BSSNaniBugNamesReplaseParser>("Nani Bug Names Parser");
        }

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            namesProperty = serializedObject.FindProperty("names");

            inputScriptPath = EditorPrefs.GetString(InputScriptPathKey, "");
            inputNarrativePath = EditorPrefs.GetString(InputNarrativePathKey, "");
            outputScriptPath = EditorPrefs.GetString(OutputScriptPathKey, "");

            string namesString = EditorPrefs.GetString(NamesKey, "");
            names = !string.IsNullOrEmpty(namesString)
                ? namesString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                : Array.Empty<string>();
        }

        private void OnDisable()
        {
            serializedObject.ApplyModifiedProperties();

            EditorPrefs.SetString(InputScriptPathKey, inputScriptPath);
            EditorPrefs.SetString(InputNarrativePathKey, inputNarrativePath);
            EditorPrefs.SetString(OutputScriptPathKey, outputScriptPath);
            EditorPrefs.SetString(NamesKey, string.Join(";", names));
        }

        private void OnGUI()
        {
            GUILayout.Label("Nani Bug Names Replace Parser", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            // Массив строк с возможностью редактирования
            GUILayout.Label("Names to Replace", EditorStyles.label);
            if (names != null)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    names[i] = EditorGUILayout.TextField($"Name {i + 1}", names[i]);
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        RemoveNameAtIndex(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("Add Name"))
            {
                AddNewName();
            }

            EditorGUILayout.Space();

            GUILayout.Label("Input Nani Script Path", EditorStyles.label);
            inputScriptPath = EditorGUILayout.TextField(inputScriptPath);
            if (GUILayout.Button("Browse Input Script"))
            {
                string selectedPath = EditorUtility.OpenFilePanel("Select Nani Script", "", "nani");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    inputScriptPath = selectedPath;
                }
            }

            GUILayout.Label("Input Narrative Nani Path", EditorStyles.label);
            inputNarrativePath = EditorGUILayout.TextField(inputNarrativePath);
            if (GUILayout.Button("Browse Input Narrative"))
            {
                string selectedPath = EditorUtility.OpenFilePanel("Select Narrative Nani", "", "nani");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    inputNarrativePath = selectedPath;
                }
            }

            GUILayout.Label("Output Parsed Script Path", EditorStyles.label);
            outputScriptPath = EditorGUILayout.TextField(outputScriptPath);
            if (GUILayout.Button("Browse Output Folder"))
            {
                string selectedPath = EditorUtility.SaveFilePanel("Save Parsed Script", "", "ParsedScript.nani", "nani");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    outputScriptPath = selectedPath;
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Convert"))
            {
                if (ValidatePaths())
                {
                    ConvertScript();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please ensure all paths are set correctly and files are .nani.", "OK");
                }
            }
        }

        private void AddNewName()
        {
            var namesList = new List<string>(names) { string.Empty };
            names = namesList.ToArray();
        }

        private void RemoveNameAtIndex(int index)
        {
            var namesList = new List<string>(names);
            if (index >= 0 && index < namesList.Count)
            {
                namesList.RemoveAt(index);
                names = namesList.ToArray();
            }
        }

        private bool ValidatePaths()
        {
            return !string.IsNullOrEmpty(inputScriptPath) && File.Exists(inputScriptPath) && Path.GetExtension(inputScriptPath) == ".nani" &&
                   !string.IsNullOrEmpty(inputNarrativePath) && File.Exists(inputNarrativePath) && Path.GetExtension(inputNarrativePath) == ".nani" &&
                   !string.IsNullOrEmpty(outputScriptPath) && Path.GetExtension(outputScriptPath) == ".nani";
        }

        private void ConvertScript()
        {
            try
            {
                string scriptContent = File.ReadAllText(inputScriptPath);
                string narrativeContent = File.ReadAllText(inputNarrativePath);

                replaceCounter = 0;
                replaseLineList = "";

                // Replace logic
                string parsedContent = ReplaceBugNames(scriptContent, narrativeContent);

                if (!outputScriptPath.EndsWith(".nani"))
                {
                    outputScriptPath += ".nani";
                }

                File.WriteAllText(outputScriptPath, parsedContent);
                EditorUtility.DisplayDialog("Success", $"The script has been parsed and saved successfully. Replace Count: {replaceCounter}", "OK");
                Debug.Log($"Replase List:\n{replaseLineList}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error during script conversion: {ex.Message}");
                EditorUtility.DisplayDialog("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private string ReplaceBugNames(string scriptContent, string narrativeContent)
        {
            // Разбиваем содержимое нарратива на строки
            string[] narrativeLines = narrativeContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Log($"narrativeLines: {narrativeLines.Length}");

            // Разбиваем содержимое скрипта на строки
            string[] scriptLines = scriptContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Log($"scriptLines: {scriptLines.Length}");

            List<string> newNaniScriptLines = new List<string>();

            for (int i = 0; i < scriptLines.Length; i++)
            {
                bool foundMatch = false;
                string lastFindNewLine = "";

                if (IsLineIsMessage(scriptLines[i]))
                {
                    for (int q = 0; q < narrativeLines.Length; q++)
                    {
                        if (IsLineIsMessage(narrativeLines[q]) &&
                            FormatNamesInLine(SplitLineItems(narrativeLines[q])[1]) == SplitLineItems(scriptLines[i])[1]
                            && IsCorrectLine(scriptLines, i, narrativeLines, q))
                        {
                            if (SplitLineItems(narrativeLines[q])[0] != SplitLineItems(scriptLines[i])[0])
                            {
                                replaseLineList += $"{scriptLines[i]} => {SplitLineItems(narrativeLines[q])[0]}: {SplitLineItems(scriptLines[i])[1]} [{i+1}]\n";
                               
                                replaceCounter++;
                            }

                            // Добавляем строку из нарратива
                            lastFindNewLine = $"{SplitLineItems(narrativeLines[q])[0]}: {SplitLineItems(scriptLines[i])[1]}";

                            // Отмечаем, что совпадение найдено
                            foundMatch = true;

                            // Не прерываем цикл, чтобы продолжить добавление других совпадений
                        }
                    }
                }

                // Если совпадения не найдено, добавляем оригинальную строку

                newNaniScriptLines.Add(foundMatch ? lastFindNewLine : scriptLines[i]);
            }

            return string.Join("\n", newNaniScriptLines);
        }
        private bool IsCorrectLine(string[] scriptLines, int scriptIndex, string[] narrativeLines, int narrativeIndex)
        {
            // Проверка на корректность начальных индексов
            if (scriptIndex < 0 || scriptIndex >= scriptLines.Length ||
                narrativeIndex < 0 || narrativeIndex >= narrativeLines.Length)
            {
                return false;
            }

            while (narrativeIndex >= 0) // Проверка, чтобы не выйти за пределы массива
            {
                narrativeIndex--;

                if (narrativeIndex < 0) break; // Если индекс становится отрицательным, выходим из цикла

                if (IsLineIsMessage(narrativeLines[narrativeIndex]))
                {
                    while (scriptIndex >= 0) // Проверка, чтобы не выйти за пределы массива
                    {
                        scriptIndex--;

                        if (scriptIndex < 0) break; // Если индекс становится отрицательным, выходим из цикла

                        if (IsLineIsMessage(scriptLines[scriptIndex]))
                        {
                            // Проверяем совпадение имен
                            if (FormatNamesInLine(SplitLineItems(narrativeLines[narrativeIndex])[1]) ==
                                SplitLineItems(scriptLines[scriptIndex])[1])
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false; // Возвращаем false, если совпадений не найдено
        }

        private bool IsLineIsMessage(string scriptLine)
        {
            string[] scriptLineSplited = SplitLineItems(scriptLine);

            if (scriptLineSplited.Length > 1)
            {
                foreach (var name in names)
                {
                    if (scriptLineSplited[0] == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private string[] SplitLineItems(string line)
        {
            return line.Split(": ");
        }

        private string FormatNamesInLine(string line)
        {
            return line.Replace("мистер Эванс", "{MainCharacter}").Replace("Мистер Эванс", "{MainCharacter}").Replace("Макс", "{MainCharacter}");
        }
    }
}
