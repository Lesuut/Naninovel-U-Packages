using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Naninovel.UFlow.Editor
{
    using Naninovel.UFlow.Data;
    using Naninovel.UFlow.Elements;
    using Naninovel.UFlow.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class FlowEditorWindow : EditorWindow
    {
        private string defoultFileName = "New Flow Graph";
        private FlowGraphView flowGraphView;

        private string currentFilePath = string.Empty;

        [MenuItem("Naninovel/U Flow/Flow Graph")]
        public static void ShowExample()
        {
            GetWindow<FlowEditorWindow>("Flow Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
        }

        private void AddGraphView()
        {
            flowGraphView = new FlowGraphView();

            flowGraphView.StretchToParentSize();

            rootVisualElement.Add(flowGraphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            Button buttonSave = new Button()
            {
                text = "Save"
            };

            Button buttonLoad = new Button()
            {
                text = "Load"
            };

            buttonSave.clicked += SaveNodes;
            buttonLoad.clicked += LoadNodes;

            toolbar.Add(buttonSave);
            toolbar.Add(buttonLoad);

            rootVisualElement.Add(toolbar);
        }

        private void ClearAllNodes()
        {
            flowGraphView.Clear();
            rootVisualElement.Clear();
            AddGraphView();
            AddToolbar();
        }

        private void SaveNodes()
        {
            if(currentFilePath == string.Empty)
            {
                string path = EditorUtility.SaveFilePanel("Save File", "", defoultFileName, "asset");

                if (!string.IsNullOrEmpty(path))
                {
                    currentFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileName(path));
                }
                else
                {
                    // Действие, если путь не выбран
                    UnityEngine.Debug.LogWarning("No file path selected for saving.");
                    return;
                }
            }

            UnityEngine.Debug.Log($"Save Nodes Path: {currentFilePath}");

            try
            {
                List<FlowNode> flowNodes = flowGraphView.GetAllNodes();

                FlowUtility.SaveNodesToAsset(currentFilePath, flowNodes.Select(item => item.Serialization()).ToList());
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to save file. Error: {ex.Message}");
            }
        }

        private void LoadNodes(FlowAsset flowNodeAsset)
        {
            ClearAllNodes();

            flowNodeAsset.LoadData();

            if (string.IsNullOrEmpty(flowNodeAsset.JsonData))
                return;

            foreach (var item in flowNodeAsset.flowNodeDatas)
            {
                flowGraphView.CreateNode(item.NodeType, new Vector2(item.NodePositionX, item.NodePositionY)).Deserialization(item);
            }
        }

        public void OpenWindowWithAsset(FlowAsset flowNodeAsset, string assetPath)
        {
            currentFilePath = assetPath;

            LoadNodes(flowNodeAsset);
        }
        private void LoadNodes()
        {
            string path = EditorUtility.OpenFilePanel("Load Flow Graph", "Assets", "asset");

            if (string.IsNullOrEmpty(path))
            {
                // User canceled or didn't select a file
                UnityEngine.Debug.LogWarning("No file path selected for loading.");
                return;
            }          

            currentFilePath = path;

            LoadNodes(FlowUtility.LoadNodesFromAsset(currentFilePath));
        }
    }
}