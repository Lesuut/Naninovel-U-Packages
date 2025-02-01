using System.Collections.Generic;
using UnityEngine;

namespace Naninovel.UFlow.Data
{
    [CreateAssetMenu(fileName = "NewFlowNodeAsset", menuName = "FlowGraph/FlowNodeAsset")]
    public class FlowAsset : ScriptableObject
    {
        public List<FlowNodeData> flowNodeDatas;
    }
}