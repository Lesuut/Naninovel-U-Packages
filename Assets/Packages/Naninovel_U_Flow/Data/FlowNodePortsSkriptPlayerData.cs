using UnityEngine;

namespace Naninovel.UFlow.Data
{
    [SerializeField]
    public class FlowNodePortsSkriptPlayerData : FlowNodeData
    {
        public string ScriptText;

        public FlowNodePortsSkriptPlayerData() { }

        public FlowNodePortsSkriptPlayerData(FlowNodeData baseData)
        {
            NodeType = baseData.NodeType;
            NodePositionX = baseData.NodePositionX;
            NodePositionY = baseData.NodePositionY;
            NodeId = baseData.NodeId;
            MapName = baseData.MapName;
        }
    }
}