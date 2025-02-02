using UnityEngine;

namespace Naninovel.UFlow.Data
{
    using Enumeration;
    using System;

    [Serializable]
    public class FlowNodeData
    {
        public FlowNodeData() { }

        public NodeType NodeType;
        public float NodePositionX;
        public float NodePositionY;
        public int NodeId;
        public string MapName;
    }
}