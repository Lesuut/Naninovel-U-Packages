using System;
using UnityEngine;

namespace Naninovel.U.Flow
{
    /// <summary>
    /// Contains configuration data for the Flow systems.
    /// </summary>
    [EditInProjectSettings]
    public class FlowConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "Flow";

        public GameObject FlowUI;
        [Space]
        [Header("Backgrounds")]
        public string BackgroundCommand = "@back %ID%";
        [Space]
        public BackgroundItem[] Backgrounds;
        [Header("Buttons")]
        public TransferButton[] TransferButtons;
        public GameObject ReturnButton;
        [Header("Flows")]
        public FlowAsset[] flowAssetsWay;

        public string API;
    }

    [Serializable]
    public struct TransferButton
    {
        public string Name;
        public GameObject Button;
    }

    [Serializable]
    public struct BackgroundItem
    {
        public string Name;
        [Tooltip("Optional")]
        public Texture2D Icone;
    }
}