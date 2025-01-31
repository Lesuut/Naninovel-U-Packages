using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.SideTip
{
    public class TipTextWindow : EditorWindow
    {
        protected string OutputPath
        {
            get => PlayerPrefs.GetString(outputPathKey, $"{Application.dataPath}/Resources/Naninovel/{ProjectConfigurationProvider.LoadOrDefault<ManagedTextConfiguration>().Loader.PathPrefix}");
            set
            {
                PlayerPrefs.SetString(outputPathKey, value);
                ValidateOutputPath();
            }
        }

        private static readonly GUIContent outputPathContent = new GUIContent("Output Path", "Path to the folder under which to sore generated managed text documents; should be `Resources/Naninovel/Text` by default.");

        private const string outputPathKey = "Naninovel." + nameof(ManagedTextWindow) + "." + nameof(OutputPath);
        private bool isWorking;
        private bool outputPathValid;
        private string pathPrefix;

        [MenuItem("Naninovel/Tools/Tip Managed Text")]
        public static void OpenWindow()
        {
            var position = new Rect(100, 100, 500, 200);
            GetWindowWithRect<TipTextWindow>(position, true, "Tip Managed Text", true);
        }

        private void OnEnable()
        {
            ValidateOutputPath();
        }

        private void ValidateOutputPath()
        {
            pathPrefix = ProjectConfigurationProvider.LoadOrDefault<ManagedTextConfiguration>().Loader.PathPrefix;
            outputPathValid = OutputPath?.EndsWith(pathPrefix) ?? false;
        }

        private void OnGUI()
        {
            Rect fullRect = new Rect(0, 0, Screen.width, Screen.height);

            EditorGUI.DrawRect(fullRect, new Color(32f / 255f, 47f / 255f, 54f / 255f));

            EditorGUI.LabelField(new Rect(0, 0, position.width, 100), "\r\n   ██╗░░░██╗  ████████╗██╗██████╗░\r\n   ██║░░░██║  ╚══██╔══╝██║██╔══██╗\r\n   ██║░░░██║  ░░░██║░░░██║██████╔╝\r\n   ██║░░░██║  ░░░██║░░░██║██╔═══╝░\r\n   ╚██████╔╝  ░░░██║░░░██║██║░░░░░\r\n   ░╚═════╝░  ░░░╚═╝░░░╚═╝╚═╝░░░░░", EditorStyles.boldLabel);
            EditorGUILayout.Space(100);
            EditorGUILayout.LabelField(
               "The tool for generating managed text documents",
               GUIStyles.RichLabelStyle
           );

            if (isWorking)
            {
                EditorGUILayout.HelpBox("Working, please wait...", MessageType.Info);
                return;
            }

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.HorizontalScope())
            {
                OutputPath = EditorGUILayout.TextField(outputPathContent, OutputPath);
                if (GUILayout.Button("Select", EditorStyles.miniButton, GUILayout.Width(65)))
                    OutputPath = EditorUtility.OpenFolderPanel("Output Path", "", "");
            }

            GUILayout.FlexibleSpace();

            if (!outputPathValid)
                EditorGUILayout.HelpBox($"Output path is not valid. Make sure it points to a `{pathPrefix}` folder stored under a `Resources` folder.", MessageType.Error);
            else if (GUILayout.Button("Generate Managed Text Documents", GUIStyles.NavigationButton))
                GenerateDocuments();
            EditorGUILayout.Space();
        }

        private void GenerateDocuments()
        {
            isWorking = true;

            if (!Directory.Exists(OutputPath))
                Directory.CreateDirectory(OutputPath);

            var records = GenerateRecords();
            var categoryToTextMap = records.GroupBy(t => t.Category).ToDictionary(t => t.Key, t => new HashSet<ManagedTextRecord>(t));

            foreach (var kv in categoryToTextMap)
                ProcessDocumentCategory(kv.Key, kv.Value);

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            isWorking = false;
            Repaint();
        }

        private void ProcessDocumentCategory(string category, HashSet<ManagedTextRecord> records)
        {
            var fullPath = $"{OutputPath}/{category}.txt";

            // Try to update existing resource.
            if (File.Exists(fullPath))
            {
                var documentText = File.ReadAllText(fullPath);
                var existingRecords = ManagedTextUtils.ParseDocument(documentText, category);
                // Remove existing fields no longer associated with the category (possibly moved to another or deleted).
                existingRecords.RemoveWhere(t => !records.Contains(t));
                // Remove new fields that already exist in the updated document, to prevent overriding.
                records.ExceptWith(existingRecords);
                // Add existing fields to the new set.
                records.UnionWith(existingRecords);
                File.Delete(fullPath);
            }

            var lines = new List<string>();
            foreach (var record in records)
                lines.Add(record.ToDocumentTextLine());
            lines = lines.OrderBy(l => l).ToList();
            var resultString = string.Join(Environment.NewLine, lines);

            File.WriteAllText(fullPath, resultString);
        }

        private static HashSet<ManagedTextRecord> GenerateRecords()
        {
            var tipConfiguration = ProjectConfigurationProvider.LoadOrDefault<TipConfiguration>().ContextKeyItems;
            string category = ProjectConfigurationProvider.LoadOrDefault<TipConfiguration>().Category;

            var records = tipConfiguration
                .Select(item => new ManagedTextRecord(item.Key, item.Value, category)) // Создаем записи
                .OrderBy(r => r.Key) // Сортируем по ключу
                .ToList(); // Преобразуем в список, чтобы использовать его в HashSet

            return new HashSet<ManagedTextRecord>(records);
        }
    }
}