using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Naninovel.U.TemplateServiceGeneratorWindow
{
    public class EmptyTemplateServiceGeneratorWindow : EditorWindow
    {
        private readonly string version = "V4";

        private bool isService = true;

        private string servicePath;
        private string coreName = "CoreName";

        private string commands;
        private string functions;

        private bool useService = true;
        private bool useConfiguration = false;

        private bool useUI;
        private bool useUIdata;

        private bool useSetting = false;

        [MenuItem("Tools/Template Service Generator")]
        public static void OpenWindow()
        {
            var window = GetWindow<EmptyTemplateServiceGeneratorWindow>("Template Nani Generator");
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(460, 410);
        }

        private void OnEnable()
        {
            servicePath = EditorPrefs.GetString("EmptyServiceGeneratorPath", string.Empty);
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), GetWindowColor());

            if (useService)
                EditorGUILayout.LabelField($"Template {(isService ? "Service" : "Manager")} Generator\t{version}", EditorStyles.boldLabel);
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
                useConfiguration = EditorGUILayout.Toggle(new GUIContent("Use Configuration"), useConfiguration);
            else
                useConfiguration = false;

            if (useConfiguration)
                useSetting = EditorGUILayout.Toggle(new GUIContent("Use Settings"), useSetting);
            else
                useSetting = false;

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
                if (!useService && !useUI && commands.Replace(" ", "").Split(",").Length > 0 && functions.Replace(" ", "").Split(",").Length > 0)
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

            string serviceFolderName = $"Naninovel_U_{coreName}";
            string serviceFolderPath = Path.Combine(servicePath, serviceFolderName);
            string sm = isService ? "Service" : "Manager";

            // Убедимся, что директория для сервиса существует
            CreateEmptyFolder(servicePath, serviceFolderName);

            // Создаем подпапки
            string runtimePath = Path.Combine(serviceFolderPath, "Runtime");

            CreateEmptyFolder(serviceFolderPath, "Runtime");

            if (useService)
            {
                GenerateCSharpScript(runtimePath, $"{coreName}State",
                        ReplaceKeys(templateServiceGeneratorInfo.BaseState.text,
                        coreName, sm));

                GenerateCSharpScript(runtimePath, $"I{coreName}{sm}",
                        ReplaceKeys(templateServiceGeneratorInfo.IBaseService.text,
                        coreName, sm));

                if (useConfiguration)
                {
                    GenerateCSharpScript(runtimePath, $"{coreName}{sm}",
                        ReplaceKeys(templateServiceGeneratorInfo.BaseServiceConfigucation.text,
                        coreName, sm));                    

                    if (useSetting)
                    {
                        GenerateCSharpScript(runtimePath, $"{coreName}Configucation",
                            ReplaceKeys(templateServiceGeneratorInfo.BaseConfigucationAPI.text,
                            coreName, sm));

                        CreateEmptyFolder(runtimePath, "Editor");

                        GenerateCSharpScript(Path.Combine(runtimePath, "Editor"), $"{coreName}Settings",
                            ReplaceKeys(templateServiceGeneratorInfo.BaseSettings.text,
                            coreName, sm));

                        GenerateCSharpScript(Path.Combine(runtimePath, "Editor"), $"SyntaxHighlighter",
                            ReplaceKeys(templateServiceGeneratorInfo.SyntaxHighlighter.text,
                            coreName, sm));
                    }
                    else
                    {
                        GenerateCSharpScript(runtimePath, $"{coreName}Configucation",
                            ReplaceKeys(templateServiceGeneratorInfo.BaseConfigucation.text,
                            coreName, sm));
                    }
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

                string uiDataTemplate;

                if (useUIdata)
                {
                    uiDataTemplate = useService
                        ? templateServiceGeneratorInfo.BaseUIData.text
                        : templateServiceGeneratorInfo.BaseUIDataNoSystem.text;
                }
                else
                {
                    uiDataTemplate = useService
                        ? templateServiceGeneratorInfo.BaseUI.text
                        : templateServiceGeneratorInfo.BaseUINoSystem.text;
                }

                GenerateCSharpScript(Path.Combine(runtimePath, "UI"), $"{coreName}UI",
                    ReplaceKeys(uiDataTemplate, coreName, sm));
            }

            if (!string.IsNullOrEmpty(commands))
            {
                CreateEmptyFolder(runtimePath, "Commands");

                foreach (var commandName in commands.Replace(" ", "").Split(","))
                {
                    if (!string.IsNullOrEmpty(commandName))
                    {
                        GenerateCSharpScript(Path.Combine(runtimePath, "Commands"), $"{CapitalizeFirstLetter(commandName)}Command",
                        ReplaceKeys(useService ? templateServiceGeneratorInfo.BaseCommand.text : templateServiceGeneratorInfo.BaseCommandEmpty.text,
                        coreName, sm).Replace("%COMMANDNAME%", commandName.ToLower()).Replace("%COMMANDNAMEHEAD%", CapitalizeFirstLetter(commandName)));
                    }
                }
            }

            if (!string.IsNullOrEmpty(functions))
            {
                List<string> functionVoids = new List<string>();

                foreach (var functionName in functions.Replace(" ", "").Split(","))
                {
                    if (!string.IsNullOrEmpty(functionName))
                    {
                        if (useService)
                        {
                            functionVoids.Add($"        " +
                                $"public static string {CapitalizeFirstLetter(functionName)}()\r\n        " +
                                $"{{\r\n            " +
                                $"var {coreName}{sm} = Engine.GetService<I{coreName}{sm}>();\r\n\r\n            " +
                                $"return \"\";\r\n        " +
                                $"}}");
                        }
                        else if (useUI)
                        {
                            functionVoids.Add($"" +
                                $"public static string {CapitalizeFirstLetter(functionName)}()\r\n        " +
                                $"{{\r\n            " +
                                $"var uiManager = Engine.GetService<IUIManager>();\r\n            " +
                                $"var {coreName}UI = uiManager.GetUI<{coreName}UI>();\r\n\r\n            " +
                                $"return \"\";\r\n        " +
                                $"}}");
                        }
                        else
                        {
                            functionVoids.Add($"" +
                                $"public static string {CapitalizeFirstLetter(functionName)}()\r\n        " +
                                $"{{\r\n            " +
                                $"return \"\";\r\n        " +
                                $"}}");
                        }
                    }
                }

                GenerateCSharpScript(runtimePath, $"{coreName}Functions",
                       $"namespace Naninovel.U.{coreName}\r\n{{\r\n    " +
                       $"[ExpressionFunctions]\r\n    " +
                       $"public static class {coreName}Functions\r\n    " +
                       $"{{\n" +
                       $"{string.Join(Environment.NewLine, functionVoids) + Environment.NewLine}" +
                       $"    }}\r\n}}");
            }

            AssetDatabase.Refresh();
        }

        private void CreateEmptyFolder(string directory, string folderName)
        {
            string folderPath = Path.Combine(directory, folderName);

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

            if (!File.Exists(scriptPath))
            {
                File.WriteAllText(scriptPath, scriptContent);
                Debug.Log($"Generated script: {scriptPath}");
            }
            else
            {
                Debug.Log($"Script already exists: {scriptPath}");
            }
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
                newColor = new Color(newColor.r, newColor.g, Mathf.Clamp01(newColor.b + (commands.Replace(" ", "").Split(',').Length * 0.07f)));

            if (!string.IsNullOrEmpty(functions) && functions.Split(',').Length >= 1)
                newColor = new Color(newColor.r, Mathf.Clamp01(newColor.g + (functions.Replace(" ", "").Split(',').Length * 0.035f)), Mathf.Clamp01(newColor.b + (functions.Split(',').Length * 0.035f)));

            if (!useService)
                newColor = new Color(Mathf.Clamp01(newColor.r + 0.2f), newColor.g, newColor.b);

            if (useConfiguration)
                newColor = new Color(Mathf.Clamp01(newColor.r + 0.05f), newColor.g, Mathf.Clamp01(newColor.b + 0.05f));

            if (useSetting)
                newColor = new Color(newColor.r, Mathf.Clamp01(newColor.g + 0.1f), Mathf.Clamp01(newColor.b + 0.1f));

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
            string path = "Assets/Packages/NaninovelEmptyTemplateServiceGenerator/Templates/TemplateServiceGeneratorInfo.asset";

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

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}