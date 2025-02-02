using Naninovel.UFlow.Data;
using System;
using System.Collections.Generic;

[Serializable]
public class FlowNodePortsData : FlowNodeData
{
    public List<FlowPortData> inputPorts;
    public List<FlowPortData> outputPorts;

    public FlowNodePortsData() { }

    public FlowNodePortsData(FlowNodeData baseData)
    {
        NodeType = baseData.NodeType;
        NodePositionX = baseData.NodePositionX;
        NodePositionY = baseData.NodePositionY;
        NodeId = baseData.NodeId;
        MapName = baseData.MapName;
    }
}