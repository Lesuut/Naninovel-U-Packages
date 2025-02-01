using UnityEngine;

namespace Naninovel.UFlow.Data
{
    using Enumeration;
    using System.Collections.Generic;

    public class FlowNodeData : ScriptableObject
    {
        public NodeType NodeType;
        public Vector2 NodePosition;
        public List<FlowPortData> OutputPorts;
        public List<FlowPortData> InputPorts;

        public FlowNodeData()
        {
            OutputPorts = new List<FlowPortData>();
            InputPorts = new List<FlowPortData>();
        }
    }
}