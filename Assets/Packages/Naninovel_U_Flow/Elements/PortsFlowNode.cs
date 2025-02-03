using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;

namespace Naninovel.UFlow.Elements
{
    using Naninovel.UFlow.Data;
    using Naninovel.UFlow.Utility;

    public class PortsFlowNode : FlowNode
    {
        private int outputPortCount = 1;
        private List<PopupField<string>> popupFields = new List<PopupField<string>>(); // Список для хранения PopupField

        protected override void SetBaseStyle()
        {
            title = $"{ID} Ports Node";
        }

        public override FlowNodeData Serialization()
        {
            var flowNodePortsData = new FlowNodePortsData(base.Serialization())
            {
                inputPorts = FlowUtility.SerializeFlowNodeConnections(inputContainer, false),
                outputPorts = FlowUtility.SerializeFlowNodeConnections(outputContainer, true),
                outputButtonsNames = popupFields.Select(item => item.value).ToList()
            };

            return flowNodePortsData;
        }

        public override void Deserialization(FlowNodeData flowNodeData)
        {
            base.Deserialization(flowNodeData);

            for (int i = 0; i < ((FlowNodePortsData)flowNodeData).outputPorts.Count - 2; i++)
            {
                AddOutputPort();
            }
            for (int i = 0; i < popupFields.Count; i++)
            {
                popupFields[i].value = ((FlowNodePortsData)flowNodeData).outputButtonsNames[i];
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

            // Сохраняем порты и popupField в соответствующие списки
            popupFields.Add(popupField);

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

                    // Удаляем сам порт и popupField
                    outputContainer.Remove(lastPort);
                    popupFields.RemoveAt(popupFields.Count - 1);
                    outputPortCount--;

                    RefreshExpandedState();
                    RefreshPorts(); // Обновляем порты
                }
            }
        }

        // Метод для получения значения из popupField
        public string GetPopupFieldValue(int portIndex)
        {
            if (portIndex >= 0 && portIndex < popupFields.Count)
            {
                return popupFields[portIndex].value; // Получаем значение выбранное в PopupField
            }
            return null;
        }
    }
}