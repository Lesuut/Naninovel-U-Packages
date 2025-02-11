using Naninovel.UFlow.Data;
using System.Linq;
using UnityEngine;

namespace Naninovel.U.Flow
{
    using Naninovel.UFlow.Enumeration;
    using System;
    using Unity.VisualScripting;

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

        public void ResetService() 
        {
            if (flowUI != null)
                flowUI.HideAllButtons();

            state.currentActiveFlowNodeId = -1;
        }

        private void Serialize(GameStateMap map)
        {
            map.SetState(new FlowState(state));
        }

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<FlowState>();
            state = state == null ? new FlowState() : new FlowState(state);

            if (state.isFlowActive)
            {
                UpdateFlowScene(state.currentFlowAssetName == "" ? 
                    Configuration.flowAssetsWay[state.currentFlowIndex] : 
                    Configuration.flowAssetsWay.FirstOrDefault(item => item.name == state.currentFlowAssetName));
            }

            return UniTask.CompletedTask;
        }

        public void StartFlow()
        {
            state.isFlowActive = true;

            state.startScriptName = scriptPlayer.Playlist.ScriptName;
            state.startScriptPlayedIndex = scriptPlayer.PlayedIndex;

            UpdateFlowScene(Configuration.flowAssetsWay[state.currentFlowIndex]);
        }
        public void StartFlowByName(string FlowAssetName)
        {
            Debug.Log($"StartFlowByName: {FlowAssetName}");

            state.isFlowActive = true;

            state.startScriptName = scriptPlayer.Playlist.ScriptName;
            state.startScriptPlayedIndex = scriptPlayer.PlayedIndex;

            state.currentFlowAssetName = FlowAssetName;

            Debug.Log(Configuration.flowAssetsWay.FirstOrDefault(item => item.name == FlowAssetName).name);

            UpdateFlowScene(Configuration.flowAssetsWay.FirstOrDefault(item => item.name == FlowAssetName));
        }

        private void UpdateFlowScene(FlowAsset flowAsset)
        {
            if (state.isFlowActive)
            {
                Debug.Log($"Start {flowAsset.name}");

                scriptPlayer.Stop();

                flowAsset.LoadData();

                if (state.currentActiveFlowNodeId == -1)
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
            TrySpawnFlowUI();
            PlayScript("@hidePrinter");
            state.currentActiveFlowNodeId = nodeForActivation.NodeId;

            flowUI.HideAllButtons();
            PlayScript($"{Configuration.BackgroundCommand.Replace("%ID%", nodeForActivation.MapName)}");

            Debug.Log($"Flow current scene node id: {nodeForActivation.NodeId}");

            if (state.customEndNodeID != -1 && nodeForActivation.NodeId == state.customEndNodeID)
            {
                state.currentFlowAssetName = "";
                state.customEndNodeID = -1;

                state.isFlowActive = false;
                flowUI.HideAllButtons();

                await scriptPlayer.PreloadAndPlayAsync(state.startScriptName);
                scriptPlayer.Play(scriptPlayer.Playlist, state.startScriptPlayedIndex + 1);
                return;
            }

            if (nodeForActivation.NodeType == NodeType.Start || nodeForActivation.NodeType == NodeType.Waypoint)
            {
                FlowNodePortsData flowNodePortsData = nodeForActivation as FlowNodePortsData;

                for (int i = 1; i < flowNodePortsData.outputPorts.Count; i++)
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

            if (nodeForActivation is FlowNodePortsData)
            {
                FlowNodePortsData flowNodePortsData = nodeForActivation as FlowNodePortsData;

                if (flowNodePortsData.NodeType != NodeType.End)
                {
                    if (flowNodePortsData.outputPorts[0].NodeId != -1)
                    {
                        foreach (var node in flowAsset.flowNodeDatas)
                        {
                            if (node.NodeType == NodeType.PlayScript)
                            {
                                if (flowNodePortsData.outputPorts[0].NodeId == node.NodeId)
                                {
                                    if (node is FlowNodePortsSkriptPlayerData)
                                    {
                                        PlayScript(((FlowNodePortsSkriptPlayerData)node).ScriptText);
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
                if (state.currentFlowAssetName.IsUnityNull())
                {
                    state.currentFlowIndex++;
                }
                else
                {
                    state.currentFlowAssetName = "";
                }

                state.isFlowActive = false;
                flowUI.HideAllButtons();

                await scriptPlayer.PreloadAndPlayAsync(state.startScriptName);
                scriptPlayer.Play(scriptPlayer.Playlist, state.startScriptPlayedIndex + 1);
            }
        }
        private async void PlayScript(string scriptText)
        {
            var script = Script.FromScriptText($"Generated script", scriptText);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
        private void TrySpawnFlowUI()
        {
            if (flowUI == null)
            {
                uIManager.AddUIAsync(Configuration.FlowUI);
                flowUI = uIManager.GetUI<FlowUI>();
            }
            else
            {
                flowUI.Show();
            }
        }

        public void SetButtonsHideStatus(bool hideStatus)
        {
            state.hideFlowButtonsStatus = hideStatus;
            flowUI.SetHideButtonsStatus(hideStatus);

            if (!hideStatus)
                PlayScript("@hidePrinter");
        }

        public void SetFlowWayIndex(int newIndex)
        {
            state.currentFlowWayIndex = newIndex;
        }

        public void SetCustomFLowEndID(int endNodeId)
        {
            state.customEndNodeID = endNodeId;
        }
    }
}