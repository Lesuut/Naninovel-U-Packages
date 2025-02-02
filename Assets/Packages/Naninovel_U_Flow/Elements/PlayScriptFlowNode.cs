using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using System;
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;

    public class PlayScriptFlowNode : FlowNode
    {
        protected override void SetBaseStyle()
        {
            NodeType = NodeType.PlayScript;
            title = $"{ID} Play Script Node";
            mainContainer.AddToClassList("flow-node-action");
        }

        protected override void TitleContainer() { }

        protected override void OutputContainer() { }

        protected override void InputContainer()
        {
            Port portNodePerent = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Action));
            portNodePerent.portName = $"Action";
            portNodePerent.AddToClassList("port");
            inputContainer.Add(portNodePerent);
        }

        protected override void ExtensionsContainer()
        {
            TextField textField = new TextField()
            {
                value = "123"                     
            };

            textField.multiline = true;
            textField.style.minHeight = 100; // Устанавливаем минимальную высоту
            textField.style.whiteSpace = WhiteSpace.Normal; // Позволяет перенос строк

            extensionContainer.Add(textField);
        }
    }
}
