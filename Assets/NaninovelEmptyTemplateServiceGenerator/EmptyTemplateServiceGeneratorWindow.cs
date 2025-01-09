using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.TemplateServiceGeneratorWindow
{
    public class EmptyTemplateServiceGeneratorWindow : EditorWindow
    {
        private string servicePath;
        private string serviceName = "NewEmptyService";

        private string commands;
        private string functions;

        private bool useService;
        private bool useConfiguration;

        private bool useUI;
        private bool useUIdata;

        [MenuItem("Tools/Template Service Generator")]
        public static void OpenWindow()
        {
            GetWindow<EmptyTemplateServiceGeneratorWindow>("Template Service Generator");
        }

        private void OnEnable()
        {
            servicePath = EditorPrefs.GetString("EmptyServiceGeneratorPath", string.Empty);
        }

        private void OnGUI()
        {
            // Set the background color
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), GetWindowColor());

            // Continue with the rest of your GUI code
            EditorGUILayout.LabelField("Template Service Generator", EditorStyles.boldLabel);

            serviceName = EditorGUILayout.TextField("Service Name", serviceName);

            EditorGUILayout.Space();

            commands = EditorGUILayout.TextField(new GUIContent("Commands", "Enter the values Names through ','"), commands);
            functions = EditorGUILayout.TextField(new GUIContent("Functions", "Enter the values Names through ','"), functions);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("———–––———–––———––––");
            useService = EditorGUILayout.Toggle(new GUIContent("Use Service"), useService);
            if (useService)
                useConfiguration = EditorGUILayout.Toggle(new GUIContent("Use Configuration"), useConfiguration);
            else
                useConfiguration = false;
            EditorGUILayout.LabelField("———–––———–––———––––");
            useUI = EditorGUILayout.Toggle(new GUIContent("Use UI"), useUI);
            if (useUI)
                useUIdata = EditorGUILayout.Toggle(new GUIContent("Use UI with Data"), useUIdata);
            else
                useUIdata = false;
            EditorGUILayout.LabelField("———–––———–––———––––");

            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                servicePath = EditorGUILayout.TextField("Save Path", servicePath);
                if (GUILayout.Button("Select", GUILayout.Width(65)))
                {
                    // Используем OpenFolderPanel для выбора каталога
                    servicePath = EditorUtility.OpenFolderPanel("Select Save Path", "", "");
                    if (!string.IsNullOrEmpty(servicePath))
                    {
                        EditorPrefs.SetString("EmptyServiceGeneratorPath", servicePath);
                    }
                }
            }

            if (GUILayout.Button("Generate Service"))
            {
                if (string.IsNullOrEmpty(servicePath))
                {
                    EditorUtility.DisplayDialog("Error", "Please specify a valid path to save the service.", "OK");
                    return;
                }

                GenerateEmptyService();
                EditorUtility.DisplayDialog("Success", "Empty Service file generated successfully.", "OK");
            }
        }

        private void GenerateEmptyService()
        {
            string serviceFolderName = $"Naninovel_{serviceName}";
            string serviceFolderPath = Path.Combine(servicePath, serviceFolderName);

            // Убедимся, что директория для сервиса существует
            CreateEmptyFolder(servicePath, serviceFolderName);

            // Создаем подпапки
            string runtimePath = Path.Combine(serviceFolderPath, "Runtime");
            CreateEmptyFolder(serviceFolderPath, "Runtime");
            CreateEmptyFolder(runtimePath, "Commands");
            CreateEmptyFolder(runtimePath, "UI");

            GenerateCSharpScript(runtimePath, "TestScript", "//Hello");
        }

        private void CreateEmptyFolder(string directory, string folderName)
        {
            string folderPath = Path.Combine(directory, folderName);

            // Убедимся, что путь существует
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log($"Created folder: {folderPath}");
            }
            else
            {
                Debug.Log($"Folder already exists: {folderPath}");
            }
        }

        private void GenerateCSharpScript(string directory, string scriptName, string scriptContent)
        {
            string scriptPath = Path.Combine(directory, scriptName + ".cs");

            // Перезаписываем существующий файл
            File.WriteAllText(scriptPath, scriptContent);
            Debug.Log($"Generated or overwritten script: {scriptPath}");
        }

        private Color GetWindowColor()
        {
            // Начальный цвет (темно-серый)
            Color newColor = new Color(0.22f, 0.22f, 0.22f);
            // Получаем яркость начального цвета
            float brightness = (newColor.r + newColor.g + newColor.b) / 3f;

            if (useConfiguration)
                newColor = new Color(newColor.r, Mathf.Clamp01(newColor.g + 0.1f), newColor.b);

            if (useUIdata)
                newColor = new Color(newColor.r, newColor.g, Mathf.Clamp01(newColor.b + 0.1f));

            if (commands.Split(',').Length > 1)
                newColor = new Color(newColor.r + (commands.Split(',').Length * 0.07f), newColor.g, Mathf.Clamp01(newColor.b));

            if (functions.Split(',').Length > 1)
                newColor = new Color(newColor.r, Mathf.Clamp01(newColor.g + (functions.Split(',').Length * 0.035f)), Mathf.Clamp01(newColor.b + (functions.Split(',').Length * 0.035f)));

            // Корректируем цвет так, чтобы яркость оставалась прежней
            float newBrightness = (newColor.r + newColor.g + newColor.b) / 3f;
            float brightnessFactor = brightness / newBrightness;

            // Применяем коэффициент яркости ко всем компонентам цвета
            newColor = new Color(
                Mathf.Clamp01(newColor.r * brightnessFactor),
                Mathf.Clamp01(newColor.g * brightnessFactor),
                Mathf.Clamp01(newColor.b * brightnessFactor)
            );

            return newColor;
        }
    }
}
