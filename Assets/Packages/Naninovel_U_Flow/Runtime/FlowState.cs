using System;
using System.Collections.Generic;

namespace Naninovel.U.Flow
{
    [Serializable]
    public class FlowState
    {
        public int currentFlowWayIndex;

        public bool isFlowActive;
        public int currentFlowIndex;
        public int currentActiveFlowNodeId;

        public FlowState()
        {
            currentFlowWayIndex = 0;
            isFlowActive = false;
            currentFlowIndex = 0;
            currentActiveFlowNodeId = 0;
        }

        public FlowState(FlowState other)
        {
            // Load and set Data
            currentFlowWayIndex = other.currentFlowWayIndex;
            isFlowActive |= other.isFlowActive;
            currentFlowIndex = other.currentFlowIndex;
            currentActiveFlowNodeId = other.currentActiveFlowNodeId;
        }
    }
}
