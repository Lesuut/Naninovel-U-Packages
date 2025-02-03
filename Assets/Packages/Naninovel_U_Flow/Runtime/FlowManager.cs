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
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.Events;

    [InitializeAtRuntime()]
    public class FlowManager : IFlowManager
    {
        public virtual FlowConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private IUIManager uIManager;
        private IScriptPlayer scriptPlayer;
        private FlowState state;

        private FlowUI flowUI;

        public FlowManager(FlowConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new FlowState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            uIManager = Engine.GetService<IUIManager>();
            scriptPlayer = Engine.GetService<IScriptPlayer>();

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

            if (state.isFlowActive)
            {
                UpdateFlowScene();
            }

            return UniTask.CompletedTask;
        }

        public void StartFlow()
        {
            state.isFlowActive  = true;
            scriptPlayer.Stop();

            if (flowUI == null)
            {
                uIManager.AddUIAsync(Configuration.FlowUI);
                flowUI = uIManager.GetUI<FlowUI>();
            }

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
        private void ActivateFlowNodeScene(FlowNodeData nodeForActivation, FlowAsset flowAsset)
        {
            SetBackground("@hidePrinter");
            state.currentActiveFlowNodeId = nodeForActivation.NodeId;

            flowUI.HideAllButtons();
            SetBackground($"{Configuration.BackgroundCommand.Replace("%ID%", nodeForActivation.MapName)}");

            if (nodeForActivation.NodeType == NodeType.Start || nodeForActivation.NodeType == NodeType.Waypoint)
            {
                FlowNodePortsData flowNodePortsData = nodeForActivation as FlowNodePortsData;

                for (int i = 0; i < flowNodePortsData.outputPorts.Count; i++)
                {
                    if (flowNodePortsData.outputPorts[i].NodeId != -1)
                    {
                        FlowNodeData gotoNode = flowAsset.flowNodeDatas.FirstOrDefault(item => item.NodeId == flowNodePortsData.outputPorts[i].NodeId);

                        flowUI.CreateTransitionButton(Configuration.TransferButtons.FirstOrDefault(
                            item => item.Name == flowNodePortsData.outputButtonsNames[i - 1]).Button, 
                            () => ActivateFlowNodeScene(gotoNode, flowAsset));
                    }
                }

                if (flowNodePortsData is FlowNodePortsReturnButtonData)
                {
                    if (((FlowNodePortsReturnButtonData)flowNodePortsData).useReturnButton)
                    {
                        foreach (var node in flowAsset.flowNodeDatas)
                        {
                            if (node is FlowNodePortsData)
                            {
                                foreach (var item in ((FlowNodePortsData)node).outputPorts)
                                {
                                    if (item.NodeId == nodeForActivation.NodeId)
                                    {
                                        flowUI.CreateTransitionButton(Configuration.ReturnButton,
                                        () => ActivateFlowNodeScene(node, flowAsset));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (nodeForActivation.NodeType == NodeType.End)
            {
                state.currentFlowIndex++;
                state.isFlowActive = false;
                flowUI.HideAllButtons();
                scriptPlayer.Play(scriptPlayer.Playlist, scriptPlayer.PlayedIndex + 1);
                return;
            }
        }
        private async void SetBackground(string scriptText)
        {
            var script = Script.FromScriptText($"Generated script", scriptText);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
    }
}
