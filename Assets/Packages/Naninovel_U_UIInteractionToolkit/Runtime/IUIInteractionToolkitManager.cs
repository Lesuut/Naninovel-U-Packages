using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    public interface IUIInteractionToolkitManager : IEngineService
    {
        public void OnPointerEnter(CursorPointingTypes cursorPointingTypes);
        public void OnPointerExit(CursorPointingTypes cursorPointingTypes);
        public void OnPointerDown(CursorPointingTypes cursorPointingTypes, bool useCatch);
        public void OnPointerUp(CursorPointingTypes cursorPointingTypes);
    }
}