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
        public Texture2D CatchCursor;
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
        [Header("Action")]
        public Texture2D ActionCursor;
        public string ActionCursorSoundEnterNanicode;
        public string ActionCursorSoundExitNanicode;
        public string ActionCursorSoundDownNanicode;
        public string ActionCursorSoundUpNanicode;
    }
}