using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using Naninovel.UFlow.Data;
    using Naninovel.UFlow.Utility;
    using System.Collections.Generic;

    public class StartFlowNode : FlowNode
    {
        private int outputPortCount = 1;

        protected override void SetBaseStyle()
        {
            NodeType = NodeType.Start;
            title = "Start Node";
            mainContainer.AddToClassList("flow-node-start");
        }

        public override FlowNodeData Serialization()
        {
            var flowNodePortsData = new FlowNodePortsData(base.Serialization())
            {
                inputPorts = FlowUtility.SerializeFlowNodeConnections(inputContainer),
                outputPorts = FlowUtility.SerializeFlowNodeConnections(outputContainer)
            };

            return flowNodePortsData;
        }

        public override void Deserialization(FlowNodeData flowNodeData)
        {
            base.Deserialization(flowNodeData);

            for (int i = 0; i < ((FlowNodePortsData)flowNodeData).outputPorts.Count - 1; i++)
            {
                AddOutputPort();
            }
        }

        protected override void OutputContainer()
        {
            base.OutputContainer();
            AddOutputPort();
        }

        protected override void ExtensionsContainer()
        {
            Button addButton = new Button(() => AddOutputPort()) { text = "Add Output" };
            extensionContainer.Add(addButton);

            Button removeButton = new Button(() => RemoveLastOutputPort()) { text = "Remove Last Output" };
            extensionContainer.Add(removeButton);
        }

        private void AddOutputPort()
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = $"Output {outputPortCount}";
            outputPort.AddToClassList("port");

            // Создаём PopupField для строк
            var popupField = new PopupField<string>("Output Type", FlowUtility.GetAllButtons(), 0); // Список строк и индекс по умолчанию
            popupField.AddToClassList("popup-field"); // Применяем стиль
            outputPort.contentContainer.Add(popupField);

            outputContainer.Add(outputPort);
            outputPortCount++;
            RefreshExpandedState();
            RefreshPorts(); // Обновляем порты
        }

        private void RemoveLastOutputPort()
        {
            if (outputContainer.childCount > 0)
            {
                // Получаем последний порт
                var lastPort = outputContainer.ElementAt(outputContainer.childCount - 1) as Port;

                if (lastPort != null)
                {
                    // Удаляем все соединения (Edges), связанные с этим портом
                    var edgesToRemove = new List<Edge>();
                    foreach (var edge in lastPort.connections)
                    {
                        edgesToRemove.Add(edge);
                    }

                    foreach (var edge in edgesToRemove)
                    {
                        edge.input.Disconnect(edge);
                        edge.output.Disconnect(edge);
                        edge.RemoveFromHierarchy();
                    }

                    // Удаляем сам порт
                    outputContainer.Remove(lastPort);
                    outputPortCount--;

                    RefreshExpandedState();
                    RefreshPorts(); // Обновляем порты
                }
            }
        }
    }
}