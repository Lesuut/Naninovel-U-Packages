using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace U.Tool
{
    public class GradientColorAssigner : EditorWindow
    {
        private Gradient colorGradient;

        [MenuItem("U Tools/Gradient Color Assigner")]
        public static void ShowWindow()
        {
            GradientColorAssigner window = GetWindow<GradientColorAssigner>("Gradient");
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 200, 80); // Размер окна
            window.InitializeGradient();
        }

        private void OnEnable()
        {
            InitializeGradient();
        }

        private void InitializeGradient()
        {
            if (colorGradient == null)
            {
                colorGradient = new Gradient();
                colorGradient.colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(new Color(1.0f, 0.75f, 0.8f), 0f), // Нежно-розовый
                    new GradientColorKey(new Color(0.7f, 0.85f, 1.0f), 1f)  // Нежно-голубой
                };
                colorGradient.alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                };
            }
        }

        private void OnGUI()
        {
            colorGradient = EditorGUILayout.GradientField(colorGradient);

            if (GUILayout.Button("Apply"))
            {
                ApplyGradient();
            }
        }

        private void ApplyGradient()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected!");
                return;
            }

            if (selectedObjects.Length == 1)
            {
                Debug.LogWarning("Select more than one object to apply the gradient.");
                return;
            }

            Undo.RecordObjects(selectedObjects, "Apply Gradient Colors");

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                float t = (float)i / (selectedObjects.Length - 1);
                Color color = colorGradient.Evaluate(t);

                ApplyColorToObject(selectedObjects[i], color);
                EditorUtility.SetDirty(selectedObjects[i]); // Обновление в редакторе
            }
        }

        private void ApplyColorToObject(GameObject obj, Color color)
        {
            if (obj.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = color;
            }
            if (obj.TryGetComponent<Image>(out var image))
            {
                image.color = color;
            }
            if (obj.TryGetComponent<Button>(out var button))
            {
                button.image.color = color;
            }
            if (obj.TryGetComponent<RawImage>(out var rawImage))
            {
                rawImage.color = color;
            }
            // Можно добавить другие компоненты, которые поддерживают цвет
        }

        [MenuItem("U Tools/Gradient Color Assigner/Apply %g")]
        private static void ApplyGradientShortcut()
        {
            GradientColorAssigner window = GetWindow<GradientColorAssigner>();
            window.ApplyGradient();
            window.Close();
        }
    }
}
