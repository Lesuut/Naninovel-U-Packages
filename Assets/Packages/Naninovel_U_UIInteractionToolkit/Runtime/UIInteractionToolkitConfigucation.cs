using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
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
        public string HoverCursorSoundDownNanicode;
        public string HoverCursorSoundUpNanicode;
        [Header("Interactive")]
        public Texture2D InteractiveCursor;
        public string InteractiveCursorSoundEnterNanicode;
        public string InteractiveCursorSoundExitNanicode;
        public string InteractiveCursorSoundDownNanicode;
        public string InteractiveCursorSoundUpNanicode;
        [Header("Examine")]
        public Texture2D ExamineCursor;
        public string ExamineCursorSoundEnterNanicode;
        public string ExamineCursorSoundExitNanicode;
        public string ExamineCursorSoundDownNanicode;
        public string ExamineCursorSoundUpNanicode;
    }
}