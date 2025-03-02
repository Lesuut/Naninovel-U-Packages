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

        public int startScriptPlayedIndex;

        public string customEndBackground;

        public FlowState()
        {
            currentFlowWayIndex = 0;

            currentFlowAssetName = string.Empty;

            isFlowActive = false;
            currentFlowIndex = 0;
            currentActiveFlowNodeId = -1;
            hideFlowButtonsStatus = false;

            startScriptPlayedIndex = 0;
            
            customEndBackground = string.Empty;
        }

        public FlowState(FlowState other)
        {
            // Load and set Data
            currentFlowWayIndex = other.currentFlowWayIndex;
            isFlowActive |= other.isFlowActive;
            currentFlowIndex = other.currentFlowIndex;
            currentActiveFlowNodeId = other.currentActiveFlowNodeId;
            hideFlowButtonsStatus = other.hideFlowButtonsStatus;
            startScriptPlayedIndex = other.startScriptPlayedIndex;
            currentFlowAssetName = other.currentFlowAssetName;
            customEndBackground = other.customEndBackground;
        }
    }
}
