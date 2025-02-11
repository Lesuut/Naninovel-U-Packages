using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    /// <summary>
    /// Contains configuration data for the UIInteractionToolkit systems.
    /// </summary>
    [EditInProjectSettings]
    public class UIInteractionToolkitConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "UIInteractionToolkit";

        [Header("Defoult")]
        public Texture2D DefoultCursor;
        [Header("Hover")]
        public Texture2D HoverCursor;
        public string HoverCursorSoundEnterNanicode;
        public string HoverCursorSoundExitNanicode;
        [Header("Interactive")]
        public Texture2D InteractiveCursor;
        public string InteractiveCursorSoundEnterNanicode;
        public string InteractiveCursorSoundExitNanicode;
        [Header("Examine")]
        public Texture2D ExamineCursor;
        public string ExamineCursorSoundEnterNanicode;
        public string ExamineCursorSoundExitNanicode;
    }
}