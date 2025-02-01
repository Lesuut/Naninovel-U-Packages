using UnityEngine;
using UnityEditor;
using Naninovel.UFlow.Editor;
using UnityEditor.Callbacks;

namespace Naninovel.UFlow.Data
{
    [CustomEditor(typeof(FlowAsset))]
    public class FlowNodeAssetEditor : UnityEditor.Editor
    {
        private static readonly string iconPath = "Assets/Packages/Naninovel_U_Flow/Images/flow_node_icon.png";

        void OnEnable()
        {
            SetAssetIcon();
        }

        private void SetAssetIcon()
        {
            var targetAsset = target as FlowAsset;

            if (System.IO.File.Exists(iconPath))
            {
                Texture2D iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
                EditorGUIUtility.SetIconForObject(targetAsset, iconTexture);
            }
            else
            {
                Debug.LogError($"Icon file not found at path: {iconPath}");
            }
        }

        // Метод для обработки двойного клика на ассет
        [OnOpenAsset(1)]
        public static bool OpenFlowGraphEditor(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID) as FlowAsset;

            if (asset != null)
            {
                // Получаем путь к файлу ассета
                string assetPath = AssetDatabase.GetAssetPath(asset);

                // Получаем или создаем окно FlowEditorWindow
                FlowEditorWindow window = EditorWindow.GetWindow<FlowEditorWindow>("Flow Graph");

                // Открываем окно и передаем ассет и его путь
                window.OpenWindowWithAsset(asset, assetPath);

                return true; // предотвращаем открытие стандартного редактора
            }

            return false;
        }
    }
}