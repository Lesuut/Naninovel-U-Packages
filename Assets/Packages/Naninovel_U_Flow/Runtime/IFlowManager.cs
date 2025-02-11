using UnityEngine;

namespace Naninovel.U.Flow
{
    public interface IFlowManager : IEngineService
    {
        public void StartFlow();
        public void StartFlowByName(string FlowAssetName);
        public void SetButtonsHideStatus(bool hideStatus);
        public void SetFlowWayIndex(int newIndex);
        public void SetCustomFLowEndID(int endNodeId);
    }
}