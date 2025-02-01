using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Naninovel.UFlow.Elements
{
    public class TestFlowNode : FlowNode
    {
        private int outputPortCount = 1;
        private enum OutputType { TypeA, TypeB, TypeC }

        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            title = "Test Node";
        }

        protected override void InputContainer()
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);
        }

        protected override void OutputContainer()
        {
            AddOutputPort();
        }

        protected override void ExtensionsContainer()
        {
            Button addButton = new Button(() => AddOutputPort()) { text = "Add Output" };
            extensionContainer.Add(addButton);
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
    }
}