using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Naninovel.UFlow.Elements;
using Naninovel.UFlow.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Naninovel.UFlow.Data;
using UnityEditor.Experimental.GraphView;

namespace Naninovel.UFlow.Editor
{
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

                int currentId = 0;
                flowNodes.ForEach(node => { node.ID = currentId++; });

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

            var nodeDict = new Dictionary<int, FlowNode>();

            foreach (var item in flowNodeAsset.flowNodeDatas)
            {
                // Создание ноды на основе данных
                var newNode = flowGraphView.CreateNode(item.NodeType, new Vector2(item.NodePositionX, item.NodePositionY));
                newNode.Deserialization(item);

                nodeDict.Add(item.NodeId, newNode);
            }

            SetConnections(nodeDict, flowNodeAsset.flowNodeDatas);
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
        private void SetConnections(Dictionary<int, FlowNode> nodeDict, List<FlowNodeData> nodeDataList)
        {
            // Проходим по всем данным узлов
            foreach (var data in nodeDataList)
            {
                // Проверяем, что данные узла содержат информацию о портах
                if (data is FlowNodePortsData portsData)
                {
                    // Получаем узел из словаря по его ID
                    if (nodeDict.TryGetValue(portsData.NodeId, out var currentNode))
                    {
                        // Обрабатываем выходные порты
                        if (portsData.outputPorts != null)
                        {
                            foreach (var outputPortData in portsData.outputPorts)
                            {
                                // Находим узел, который подключен к текущему выходному порту
                                if (nodeDict.TryGetValue(outputPortData.NodeId, out var connectedNode))
                                {
                                    // Создаем соединение между портами
                                    CreateConnection(currentNode, outputPortData.PortId, connectedNode, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateConnection(FlowNode outputNode, int outputPortId, FlowNode inputNode, int inputPortId)
        {
            // Получаем выходной порт из узла-источника
            var outputPort = outputNode.outputContainer.Query<Port>().ToList()[outputPortId];

            // Получаем входной порт из узла-приемника
            var inputPort = inputNode.inputContainer.Query<Port>().ToList()[inputPortId];

            // Создаем соединение (Edge) между портами
            var edge = new Edge
            {
                output = outputPort,
                input = inputPort
            };

            // Добавляем соединение в GraphView
            flowGraphView.AddElement(edge);

            // Обновляем соединения
            edge.input.Connect(edge);
            edge.output.Connect(edge);
        }
    }
}