using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Naninovel.UFlow.Editor
{
    public class FlowEditorWindow : EditorWindow
    {
        [MenuItem("Naninovel/U Flow/Flow Graph")]
        public static void ShowExample()
        {
            GetWindow<FlowEditorWindow>("Flow Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
        }

        private void AddGraphView()
        {
            FlowGraphView flowGraphView = new FlowGraphView();

            flowGraphView.StretchToParentSize();

            rootVisualElement.Add(flowGraphView);
        }
    }
}