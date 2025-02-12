using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    [EditInProjectSettings]
    public class UIInteractionToolkitConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "UIInteractionToolkit";

        [Header("Defoult")]
        public Texture2D DefoultCursor;
        public Texture2D CatchCursor;
        [Space]
        public UIInteractionItem[] UIInteractionItems = new UIInteractionItem[1] { 
        new UIInteractionItem(){
            Type = CursorPointingTypes.Hover,
        } };
    }
}