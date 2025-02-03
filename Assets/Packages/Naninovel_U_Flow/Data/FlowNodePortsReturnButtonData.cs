using UnityEngine;

namespace Naninovel.UFlow.Data
{
    [SerializeField]
    public class FlowNodePortsReturnButtonData : FlowNodePortsData
    {
        public bool useReturnButton;

        public FlowNodePortsReturnButtonData() { }

        public FlowNodePortsReturnButtonData(FlowNodePortsData baseData)
            : base(baseData)
        {
            inputPorts = baseData.inputPorts;
            outputPorts = baseData.outputPorts;
            outputButtonsNames = baseData.outputButtonsNames;
        }
    }
}