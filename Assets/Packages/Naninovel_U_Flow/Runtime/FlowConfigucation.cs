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

        [Header("Backgrounds")]
        public string BackgroundCommand = "@back %ID%";
        [Space]
        public string[] Backgrounds;
        [Header("Buttons")]
        public TransferButton[] TransferButtons;
    }

    [Serializable]
    public struct TransferButton
    {
        public string Name;
        public GameObject Button;
    }
}