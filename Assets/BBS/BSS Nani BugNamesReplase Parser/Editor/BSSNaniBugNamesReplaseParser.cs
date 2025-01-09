using Codice.Client.BaseCommands.Changelist;
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

        // Массив строк, которые будем редактировать в окне
        public string[] names;

        private SerializedObject serializedObject;
        private SerializedProperty namesProperty;

        private int replaceCounter = 0;

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
            // Инициализация SerializedObject и SerializedProperty для работы с массивом
            serializedObject = new SerializedObject(this);
            namesProperty = serializedObject.FindProperty("names");

            // Загрузка данных из EditorPrefs
            inputScriptPath = EditorPrefs.GetString(InputScriptPathKey, "");
            inputNarrativePath = EditorPrefs.GetString(InputNarrativePathKey, "");
            outputScriptPath = EditorPrefs.GetString(OutputScriptPathKey, "");
            string namesString = EditorPrefs.GetString(NamesKey, "");
            if (!string.IsNullOrEmpty(namesString))
            {
                names = namesString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private void OnDisable()
        {
            // Применить изменения, если они были сделаны
            serializedObject.ApplyModifiedProperties();

            // Сохранение данных в EditorPrefs
            EditorPrefs.SetString(InputScriptPathKey, inputScriptPath);
            EditorPrefs.SetString(InputNarrativePathKey, inputNarrativePath);
            EditorPrefs.SetString(OutputScriptPathKey, outputScriptPath);
            if (names != null)
            {
                EditorPrefs.SetString(NamesKey, string.Join(";", names));
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Nani Bug Names Replace Parser", EditorStyles.boldLabel);

            // Добавление и редактирование массива строк
            EditorGUILayout.PropertyField(namesProperty, new GUIContent("Extracted Names"), true);

            // Применение изменений после редактирования массива
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            // Ввод путей к файлам
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

            EditorGUILayout.Space();

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

            EditorGUILayout.Space();

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

            // Кнопка для конвертации
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

            EditorGUILayout.Space();
        }

        private bool ValidatePaths()
        {
            bool isValid = !string.IsNullOrEmpty(inputScriptPath) && File.Exists(inputScriptPath) && Path.GetExtension(inputScriptPath) == ".nani" &&
                           !string.IsNullOrEmpty(inputNarrativePath) && File.Exists(inputNarrativePath) && Path.GetExtension(inputNarrativePath) == ".nani" &&
                           !string.IsNullOrEmpty(outputScriptPath) && Path.GetExtension(outputScriptPath) == ".nani";

            if (!isValid)
            {
                EditorUtility.DisplayDialog("Error", "Some paths are invalid. Please check if the files exist and are .nani.", "OK");
            }
            return isValid;
        }

        private void ConvertScript()
        {
            try
            {
                string scriptContent = File.ReadAllText(inputScriptPath);
                string narrativeContent = File.ReadAllText(inputNarrativePath);

                replaceCounter = 0;

                // Replace logic
                string parsedContent = ReplaceBugNames(scriptContent, narrativeContent);

                if (!outputScriptPath.EndsWith(".nani"))
                {
                    outputScriptPath += ".nani";
                }

                File.WriteAllText(outputScriptPath, parsedContent);
                EditorUtility.DisplayDialog("Success", $"The script has been parsed and saved successfully. Replace Count: {replaceCounter}", "OK");
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

            foreach (string naniScriptLine in scriptLines)
            {
                string modifiedLine = naniScriptLine; // Начинаем с оригинальной строки
                bool replaced = false; // Флаг для отслеживания, была ли строка изменена

                if (isLineIsMessage(GetLineItems(naniScriptLine)))
                {
                    string[] massageLine = GetLineItems(naniScriptLine);

                    foreach (string narativeLine in narrativeLines)
                    {
                        if (isLineIsMessage(GetLineItems(narativeLine)))
                        {
                            string[] narrativeLineItems = GetLineItems(narativeLine);
                            if (massageLine.Length > 1 && narrativeLineItems.Length > 1)
                            {
                                if (massageLine[1] == narrativeLineItems[1].Replace("Макс", "{MainCharacter}").Replace("Эванс", "{MainCharacter}"))
                                {
                                    replaceCounter++;
                                    modifiedLine = $"{narrativeLineItems[0]}:{massageLine[1]}"; // Изменяем строку
                                    replaced = true; // Устанавливаем флаг, что произошла замена
                                    break; // Прерываем цикл, так как мы уже нашли совпадение
                                }
                            }
                        }
                    }
                }

                // Добавляем измененную или оригинальную строку
                newNaniScriptLines.Add(modifiedLine);
            }

            return string.Join("\n", newNaniScriptLines);
        }

        private bool isLineIsMessage(string[] lineItems)
        {
            if (lineItems.Length == 2 && !string.IsNullOrEmpty(lineItems[0]))
            {
                var nameSet = new HashSet<string>(names);  // предполагается, что names - это список
                return nameSet.Contains(lineItems[0]);
            }
            return false;
        }

        private string[] GetLineItems(string line)
        {
            return line.Split(':');
        }
    }
}
