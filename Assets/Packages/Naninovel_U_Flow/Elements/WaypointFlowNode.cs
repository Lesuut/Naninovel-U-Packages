using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using Naninovel.UFlow.Data;
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;

    public class WaypointFlowNode : StartFlowNode
    {
        protected Toggle useReturnButtonToggle;

        protected override void SetBaseStyle()
        {
            NodeType = NodeType.Waypoint;
            title = $"{ID} Waypoint Node";
            mainContainer.AddToClassList("flow-node-waypoint");
        }

        protected override void InputContainer()
        {
            base.InputContainer();

            Port portNodePerent = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            portNodePerent.portName = $"Perent";
            portNodePerent.AddToClassList("port");
            inputContainer.Add(portNodePerent);
        }

        protected override void ExtensionsContainer()
        {
            base.ExtensionsContainer();

            useReturnButtonToggle = new Toggle("Use Return Button"); // Название переключателя
            useReturnButtonToggle.value = true;

            extensionContainer.Add(useReturnButtonToggle);
        }

        public override FlowNodeData Serialization()
        {
            if (base.Serialization() is FlowNodePortsData portsData)
            {
                var flowNodePortsData = new FlowNodePortsReturnButtonData(portsData)
                {
                    useReturnButton = useReturnButtonToggle.value
                };

                return flowNodePortsData;
            }

            Debug.LogError("Ошибка сериализации: base.Serialization() не является FlowNodePortsData.");
            return null;
        }

        public override void Deserialization(FlowNodeData flowNodeData)
        {
            base.Deserialization(flowNodeData);

            useReturnButtonToggle.value = ((FlowNodePortsReturnButtonData)flowNodeData).useReturnButton;
        }
    }
}
