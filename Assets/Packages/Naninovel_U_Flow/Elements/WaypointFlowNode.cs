using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using UnityEditor.Experimental.GraphView;

    public class WaypointFlowNode : StartFlowNode
    {
        protected override void SetBaseStyle()
        {
            NodeType = NodeType.Waypoint;
            title = "Waypoint Node";
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
    }
}
