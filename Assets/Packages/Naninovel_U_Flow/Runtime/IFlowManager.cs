using UnityEngine;

namespace Naninovel.U.Flow
{
    public interface IFlowManager : IEngineService
    {
        public void StartFlow();
        public void StartFlow(string FlowAssetName);
    }
}