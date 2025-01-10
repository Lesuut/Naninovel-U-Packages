using System.IO;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.TemplateServiceGeneratorWindow
{
    public class EmptyTemplateServiceGeneratorWindow : EditorWindow
    {
        private bool isService = true;

        private string servicePath;
        private string coreName = "NewEmptySystem";

        private string commands;
        private string functions;

        private bool useService = true;
        private bool useConfigucation = false;

        private bool useUI;
        private bool useUIdata;

        [MenuItem("Tools/Template Service Generator")]
        public static void OpenWindow()
        {
            GetWindow<EmptyTemplateServiceGeneratorWindow>("Template Nani Generator");
        }

        private void OnEnable()
        {
            servicePath = EditorPrefs.GetString("EmptyServiceGeneratorPath", string.Empty);
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), GetWindowColor());

            if (useService)
                EditorGUILayout.LabelField($"Template {(isService ? "Service" : "Manager")} Generator", EditorStyles.boldLabel);
            else
                EditorGUILayout.LabelField($"Template Generator", EditorStyles.boldLabel);

            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true // Включаем поддержку Rich Text
            };

            if (useService)
            {
                EditorGUILayout.LabelField(
                    $"———–––———–––———––––\n" +
                    $"Characteristic\t<b><color=yellow>{(isService ? "Service" : "Manager")}</color></b>\n" +
                    $"Role\t\t{(isService ? "Performs a single task" : "Coordinates and manages systems")}\n" +
                    $"Focus\t\t{(isService ? "Provides functionality" : "Manages state")}\n" +
                    $"Dependencies\t{(isService ? "Minimal" : "Many dependencies")}\n" +
                    $"———–––———–––———––––",
                    richTextStyle,
                    GUILayout.Height(80), // Задаем высоту
                    GUILayout.ExpandWidth(true));
            }

            if (useService)
                isService = EditorGUILayout.Toggle(new GUIContent($"Is {(isService ? "Service" : "Manager")}"), isService);
            coreName = EditorGUILayout.TextField($"Core Name", coreName);

            EditorGUILayout.Space();

            commands = EditorGUILayout.TextField(new GUIContent("Commands", "Enter the values Names through ','"), commands);
            functions = EditorGUILayout.TextField(new GUIContent("Functions", "Enter the values Names through ','"), functions);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("———–––———–––———––––");
            useService = EditorGUILayout.Toggle(new GUIContent($"Use {(isService ? "Service" : "Manager")}"), useService);
            if (useService)
                useConfigucation = EditorGUILayout.Toggle(new GUIContent("Use Configucation"), useConfigucation);
            else
                useConfigucation = false;
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
                if (!useService && !useUI)
                {
                    EditorUtility.DisplayDialog("Nice Try", "You should decide what to do.)", "OK");
                    return;
                }

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
            TemplateServiceGeneratorInfo templateServiceGeneratorInfo = LoadTemplateServiceGeneratorInfo();

            string serviceFolderName = $"Naninovel_{coreName}";
            string serviceFolderPath = Path.Combine(servicePath, serviceFolderName);
            string sm = isService ? "Service" : "Manager";

            // Убедимся, что директория для сервиса существует
            CreateEmptyFolder(servicePath, serviceFolderName);

            // Создаем подпапки
            string runtimePath = Path.Combine(serviceFolderPath, "Runtime");

            CreateEmptyFolder(serviceFolderPath, "Runtime");
            CreateEmptyFolder(runtimePath, "Commands");

            if (useUIdata || useService)
            {
                GenerateCSharpScript(runtimePath, $"{coreName}State",
                        ReplaceKeys(templateServiceGeneratorInfo.BaseState.text,
                        coreName, sm));
            }

            if (useService)
            {
                if (useConfigucation)
                {
                    GenerateCSharpScript(runtimePath, $"{coreName}Configucation",
                        ReplaceKeys(templateServiceGeneratorInfo.BaseConfigucation.text,
                        coreName, sm));

                    GenerateCSharpScript(runtimePath, $"{coreName}{sm}", 
                        ReplaceKeys(templateServiceGeneratorInfo.BaseServiceConfigucation.text, 
                        coreName, sm));
                }          
                else
                {
                    GenerateCSharpScript(runtimePath, $"{coreName}{sm}",
                       ReplaceKeys(templateServiceGeneratorInfo.BaseService.text,
                       coreName, sm));
                }
            }

            if (useUI)
            {
                CreateEmptyFolder(runtimePath, "UI");

                GenerateCSharpScript(Path.Combine(runtimePath, "UI"), $"{coreName}UI", ReplaceKeys((useUIdata ? 
                    templateServiceGeneratorInfo.BaseUIData.text : 
                    templateServiceGeneratorInfo.BaseUI.text), 
                    coreName, sm));
            }

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
            if (commands == null) commands = string.Empty;
            if (functions == null) functions = string.Empty;

            // Начальный цвет (темно-серый)
            Color newColor = new Color(0.22f, 0.22f, 0.22f);
            // Получаем яркость начального цвета
            float brightness = (newColor.r + newColor.g + newColor.b) / 3f;

            if (!isService)
                newColor = new Color(newColor.r, Mathf.Clamp01(newColor.g + 0.1f), newColor.b);

            if (useUIdata)
                newColor = new Color(newColor.r, newColor.g, Mathf.Clamp01(newColor.b + 0.1f));

            if (!string.IsNullOrEmpty(commands) && commands.Split(',').Length >= 1)
                newColor = new Color(newColor.r, newColor.g, Mathf.Clamp01(newColor.b + (commands.Split(',').Length * 0.07f)));

            if (!string.IsNullOrEmpty(functions) && functions.Split(',').Length >= 1)
                newColor = new Color(newColor.r, Mathf.Clamp01(newColor.g + (functions.Split(',').Length * 0.035f)), Mathf.Clamp01(newColor.b + (functions.Split(',').Length * 0.035f)));

            if (!useService)
                newColor = new Color(Mathf.Clamp01(newColor.r + 0.2f), newColor.g, newColor.b);

            if (useConfigucation)
                newColor = new Color(Mathf.Clamp01(newColor.r + 0.05f), newColor.g, Mathf.Clamp01(newColor.b + 0.05f));

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

        private TemplateServiceGeneratorInfo LoadTemplateServiceGeneratorInfo()
        {
            // Путь к ScriptableObject относительно папки "Assets"
            string path = "Assets/NaninovelEmptyTemplateServiceGenerator/Templates/TemplateServiceGeneratorInfo.asset";

            // Загружаем ScriptableObject по указанному пути
            TemplateServiceGeneratorInfo templateServiceGeneratorInfo = AssetDatabase.LoadAssetAtPath<TemplateServiceGeneratorInfo>(path);

            if (templateServiceGeneratorInfo != null)
            {
                return templateServiceGeneratorInfo;
            }
            else
            {
                Debug.LogError("Failed to load ScriptableObject at path: " + path);
                return null;
            }
        }

        private string ReplaceKeys(string scriptTemple, string coreName, string SM)
        {
            return scriptTemple.Replace("%CORENAME%", coreName).Replace("%SM%", SM);
        }
    }
}
