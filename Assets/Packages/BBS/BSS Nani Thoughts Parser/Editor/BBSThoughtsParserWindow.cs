using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.BBSParserThoughtsNarrative
{
    public class BBSThoughtsParserWindow : EditorWindow
    {
        private string inputCsvPath; // New field for CSV narrative path
        private string inputNaniPath;
        private string outputTextPath;

        private const string InputNaniPathKey = "BBSParserInputNaniPath";
        private const string OutputTextPathKey = "BBSParserOutputTextPath";
        private const string InputCsvPathKey = "BBSParserInputCsvPath"; // New key for CSV narrative path

        [MenuItem("Tools/NaniScript Thoughts Parser")]
        public static void OpenWindow()
        {
            GetWindow<BBSThoughtsParserWindow>("NaniScript Thoughts Parser");
        }

        private void OnEnable()
        {
            inputNaniPath = EditorPrefs.GetString(InputNaniPathKey, string.Empty);
            outputTextPath = EditorPrefs.GetString(OutputTextPathKey, string.Empty);
            inputCsvPath = EditorPrefs.GetString(InputCsvPathKey, string.Empty); // Load CSV path from EditorPrefs
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("BBS Thoughts NaniScript Parser", EditorStyles.boldLabel);

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

            EditorGUILayout.Space();

            // Input Nani Path
            using (new EditorGUILayout.HorizontalScope())
            {
                inputNaniPath = EditorGUILayout.TextField("Input Nani Path", inputNaniPath);
                if (GUILayout.Button("Select", GUILayout.Width(65)))
                {
                    inputNaniPath = EditorUtility.OpenFolderPanel("Select Input Folder", "", "");
                    if (!string.IsNullOrEmpty(inputNaniPath))
                    {
                        EditorPrefs.SetString(InputNaniPathKey, inputNaniPath);
                    }
                }
            }

            // Output Text Path
            using (new EditorGUILayout.HorizontalScope())
            {
                outputTextPath = EditorGUILayout.TextField("Output Text Path", outputTextPath);
                if (GUILayout.Button("Select", GUILayout.Width(65)))
                {
                    outputTextPath = EditorUtility.OpenFolderPanel("Select Output Folder", "", "");
                    if (!string.IsNullOrEmpty(outputTextPath))
                    {
                        EditorPrefs.SetString(OutputTextPathKey, outputTextPath);
                    }
                }
            }

            if (GUILayout.Button("Convert"))
            {
                if (string.IsNullOrEmpty(inputNaniPath) || string.IsNullOrEmpty(outputTextPath) || string.IsNullOrEmpty(inputCsvPath))
                {
                    EditorUtility.DisplayDialog("Error", "Please specify all paths: input Nani, output text, and CSV narrative.", "OK");
                    return;
                }

                ConvertAllNaniFiles(inputNaniPath, outputTextPath, inputCsvPath);
                EditorUtility.DisplayDialog("Success", $"All Nani files successfully parsed and saved.", "OK");
            }
        }

        private List<string> ReadNaniFile(string naniPath)
        {
            if (!File.Exists(naniPath))
                throw new FileNotFoundException("Nani file not found.", naniPath);

            var lines = File.ReadAllLines(naniPath);

            return MagicParseLine(lines.ToList());
        }

        private List<string> MagicParseLine(List<string> lines)
        {
            List<string[]> narrative = LoadCsvData(inputCsvPath);
            List<string> newLines = new List<string>();

            foreach (var naniScriptLine in lines)
            {
                string newLineForAdd = naniScriptLine;

                foreach (var narrativeLine in narrative)
                {

                    if (narrativeLine.Length > 1 && narrativeLine[1] != null &&
                        !naniScriptLine.Contains("@") && naniScriptLine.Contains(narrativeLine[1].Replace("\"", "")) && narrativeLine[1].Contains("\""))
                    {
                        newLineForAdd = "Thoughts" + newLineForAdd;
                        break;
                    }
                }   

                newLines.Add(newLineForAdd);
            }

            return newLines;
        }

        private void ConvertAllNaniFiles(string inputFolderPath, string outputFolderPath, string csvFolderPath)
        {
            var files = Directory.GetFiles(inputFolderPath, "*.nani");

            foreach (var file in files)
            {
                var processedText = ReadNaniFile(file);

                string outputTextFilePath = Path.Combine(outputFolderPath, Path.GetFileName(file));

                if (File.Exists(outputTextFilePath))
                {
                    if (EditorUtility.DisplayDialog("File Already Exists",
                        $"The file \"{outputTextFilePath}\" already exists. Do you want to overwrite it?",
                        "Yes", "No"))
                    {
                        SaveTextFile(outputTextFilePath, processedText);
                    }
                    else
                    {
                        Debug.Log("Text file not overwritten.");
                    }
                }
                else
                {
                    SaveTextFile(outputTextFilePath, processedText);
                }
            }
        }

        private void SaveTextFile(string path, List<string> lines)
        {
            File.WriteAllLines(path, lines);
        }

        private List<string[]> LoadCsvData(string csvPath)
        {
            if (!File.Exists(csvPath))
                throw new FileNotFoundException("CSV file not found.", csvPath);

            var lines = File.ReadAllLines(csvPath);
            var matrixContent = new List<string[]>();

            // Split each line in the CSV file by the delimiter (splitChar) and store the results in a list of string arrays
            foreach (var line in lines)
            {
                var elements = line.Split(';');
                matrixContent.Add(elements); // Add the array of strings to the matrixContent list
            }

            return matrixContent; // Return the list of string arrays
        }
    }
}
