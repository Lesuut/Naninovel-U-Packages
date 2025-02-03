namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using UnityEngine.UIElements;

    public class StartFlowNode : PortsFlowNode
    {
        protected override void SetBaseStyle()
        {
            NodeType = NodeType.Start;
            title = $"{ID} Start Node";
            mainContainer.AddToClassList("flow-node-start");
        }
    }
}