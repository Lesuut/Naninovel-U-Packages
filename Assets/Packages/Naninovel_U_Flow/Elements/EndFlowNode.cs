namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using UnityEditor.Experimental.GraphView;

    public class EndFlowNode : StartFlowNode
    {
        protected override void SetBaseStyle()
        {
            NodeType = NodeType.End;
            title = "End Node";
            mainContainer.AddToClassList("flow-node-edn");
        }

        protected override void InputContainer()
        {
            Port portNodePerent = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            portNodePerent.portName = $"Perent";
            portNodePerent.AddToClassList("port");
            inputContainer.Add(portNodePerent);
        }

        protected override void OutputContainer() { }
        protected override void ExtensionsContainer() { }
    }
}
