using Naninovel.UFlow.Data;
using System.Collections.Generic;
public class FlowNodePortsData : FlowNodeData
{
    public List<FlowPortData> inputPorts;
    public List<FlowPortData> outputPorts;

    public FlowNodePortsData(FlowNodeData baseData)
    {
        NodeType = baseData.NodeType;
        NodePosition = baseData.NodePosition;
        NodeId = baseData.NodeId;
        MapName = baseData.MapName;
    }
}