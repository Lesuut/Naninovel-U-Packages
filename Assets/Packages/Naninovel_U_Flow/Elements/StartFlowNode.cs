using Naninovel.UFlow.Data;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using Naninovel.UFlow.Utility;

    public class StartFlowNode : FlowNode
    {
        private int outputPortCount = 1;
        private string[] outputTypes = { "TypeA", "TypeB", "TypeC" }; // Список строк вместо enum

        protected override void SetBaseStyle()
        {
            NodeType = NodeType.Start;
            title = "Start Node";
            mainContainer.AddToClassList("flow-node-start");
        }

        public override FlowNodeData Serialization()
        {
            var data = base.Serialization(); // Сначала получаем общие данные о ноде

            foreach (var port in outputContainer.Query<Port>().ToList())
            {
                // Находим первый PopupField в контент-контейнере порта
                var popupField = port.contentContainer.Query<PopupField<string>>().ToList()[0];
                if (popupField != null)
                {
                    // Извлекаем выбранный тип из PopupField
                    string selectedType = popupField.value;

                    // Сериализуем порты, добавляя тип и имя порта
                    data.OutputPorts.Add(new FlowPortData()
                    {
                        PortName = port.portName,
                        PortType = selectedType // Сохраняем строковое представление типа
                    });
                }
            }

            return data;
        }

        protected override void OutputContainer()
        {
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
                // Удаляем последний порт
                var lastPort = outputContainer.ElementAt(outputContainer.childCount - 1);
                outputContainer.Remove(lastPort);
                outputPortCount--;
                RefreshExpandedState();
                RefreshPorts(); // Обновляем порты
            }
        }
    }
}