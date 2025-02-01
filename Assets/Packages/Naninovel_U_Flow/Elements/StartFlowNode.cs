using UnityEditor.Experimental.GraphView;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using UnityEngine.UIElements;

    public class StartFlowNode : FlowNode
    {
        private int outputPortCount = 1;
        private enum OutputType { TypeA, TypeB, TypeC }

        protected override void SetBaseStyle()
        {
            NodeType = NodeType.Start;
            title = "Start Node";
            mainContainer.AddToClassList("flow-node-start");
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

            EnumField enumField = new EnumField(OutputType.TypeA);
            enumField.AddToClassList("enum-field"); // Применяем стиль
            outputPort.contentContainer.Add(enumField);

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