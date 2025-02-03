using Naninovel.UFlow.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Naninovel.U.Flow
{
    using Naninovel.UFlow.Enumeration;
    using Naninovel.UI;
    using System;
    using System.Collections.Generic;

    [InitializeAtRuntime()]
    public class FlowManager : IFlowManager
    {
        public virtual FlowConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private IUIManager uIManager;
        private IScriptPlayer scriptPlayer;
        private FlowState state;

        private List<IManagedUI> managedUIButtons;

        public FlowManager(FlowConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
            managedUIButtons = new List<IManagedUI>();
        }
        public UniTask InitializeServiceAsync()
        {
            state = new FlowState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            uIManager = Engine.GetService<IUIManager>();          

            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() { }

        private void Serialize(GameStateMap map) => map.SetState(new FlowState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<FlowState>();
            state = state == null ? new FlowState() : new FlowState(state);

            return UniTask.CompletedTask;
        }

        public void StartFlow()
        {
            state.isFlowActive  = true;
            //uIManager.AddUIAsync(Configuration.TransferButtons[0].Button);
            UpdateFlowScene();
        }
        public void StartFlow(string FlowAssetName)
        {
            throw new NotImplementedException();
        }

        private void UpdateFlowScene()
        {
            if (state.isFlowActive)
            {
                FlowAsset flowAsset = Configuration.flowAssetsWay[state.currentFlowIndex];
                flowAsset.LoadData();

                if (state.currentActiveFlowNodeId == 0)
                {
                    var startNode = flowAsset.flowNodeDatas.FirstOrDefault(item => item.NodeType == NodeType.Start);

                    if (startNode == null)
                    {
                        throw new InvalidOperationException("Start node not found!");
                    }

                    ActivateFlowNodeScene(startNode, flowAsset);
                }
                else
                {
                    ActivateFlowNodeScene(
                        flowAsset.flowNodeDatas.FirstOrDefault(item => item.NodeId == state.currentActiveFlowNodeId), 
                        flowAsset);
                }
            }
        }
        private async void ActivateFlowNodeScene(FlowNodeData nodeForActivation, FlowAsset flowAsset)
        {
            state.currentActiveFlowNodeId = nodeForActivation.NodeId;

            SetBackground(nodeForActivation.MapName);

            Debug.Log($"MovieUI: {uIManager.GetUI<MovieUI>()}");

            if (nodeForActivation.NodeType == NodeType.Start || nodeForActivation.NodeType == NodeType.Waypoint)
            {
                FlowNodePortsData flowNodePortsData = nodeForActivation as FlowNodePortsData;

                for (int i = 0; i < flowNodePortsData.outputPorts.Count; i++)
                {
                    if (flowNodePortsData.outputPorts[i].NodeId != -1)
                    {
                        Debug.Log(flowNodePortsData.outputButtonsNames[i - 1]);

                        /*IManagedUI newButton = await uIManager.AddUIAsync(Configuration.TransferButtons.FirstOrDefault(
                            item => item.Name == flowNodePortsData.outputButtonsNames[i] + 1).Button,
                            flowNodePortsData.outputButtonsNames[i]);

                        managedUIButtons.Add(newButton);*/
                    }
                }
            }
        }
        private async void SetBackground(string backgroundName)
        {
            var script = Script.FromScriptText($"SetBackground {backgroundName} generated script", $"{Configuration.BackgroundCommand.Replace("%ID%", backgroundName)}");
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
    }
}
