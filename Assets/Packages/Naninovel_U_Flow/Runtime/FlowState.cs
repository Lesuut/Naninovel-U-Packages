using System;

namespace Naninovel.U.Flow
{
    [Serializable]
    public class FlowState
    {
        public int currentFlowWayIndex;

        public string currentFlowAssetName;

        public bool isFlowActive;
        public int currentFlowIndex;
        public int currentActiveFlowNodeId;

        public bool hideFlowButtonsStatus;

        public string startScriptName;
        public int startScriptPlayedIndex;

        public int customEndNodeID;

        public FlowState()
        {
            currentFlowWayIndex = 0;

            currentFlowAssetName = string.Empty;

            isFlowActive = false;
            currentFlowIndex = 0;
            currentActiveFlowNodeId = -1;
            hideFlowButtonsStatus = false;

            startScriptName = string.Empty;
            startScriptPlayedIndex = 0;
            
            customEndNodeID = -1;
        }

        public FlowState(FlowState other)
        {
            // Load and set Data
            currentFlowWayIndex = other.currentFlowWayIndex;
            isFlowActive |= other.isFlowActive;
            currentFlowIndex = other.currentFlowIndex;
            currentActiveFlowNodeId = other.currentActiveFlowNodeId;
            hideFlowButtonsStatus = other.hideFlowButtonsStatus;
            startScriptName = other.startScriptName;
            startScriptPlayedIndex = other.startScriptPlayedIndex;
            currentFlowAssetName = other.currentFlowAssetName;
            customEndNodeID = other.customEndNodeID;
        }
    }
}
