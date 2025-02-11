using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    [InitializeAtRuntime()]
    public class UIInteractionToolkitManager : IUIInteractionToolkitManager
    {
        public virtual UIInteractionToolkitConfiguration Configuration { get; }

        public UIInteractionToolkitManager(UIInteractionToolkitConfiguration config)
        {
            Configuration = config;
        }
        public UniTask InitializeServiceAsync()
        {
            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            SetCursor(CursorPointingTypes.Defoult);
        }

        public void ResetService() 
        {
            SetCursor(CursorPointingTypes.Defoult);
        }

        private async void PlayScript(string scriptText)
        {
            if (string.IsNullOrEmpty(scriptText)) return;
            var script = Script.FromScriptText($"Generated UIInteractionToolkitManager script", scriptText);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
        private void SetCursor(CursorPointingTypes cursorPointingTypes)
        {
            switch (cursorPointingTypes)
            {
                case CursorPointingTypes.Defoult:
                    Cursor.SetCursor(Configuration.DefoultCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorPointingTypes.Hover:
                    Cursor.SetCursor(Configuration.HoverCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorPointingTypes.Interactive:
                    Cursor.SetCursor(Configuration.InteractiveCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorPointingTypes.Examine:
                    Cursor.SetCursor(Configuration.ExamineCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(Configuration.DefoultCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }

        public void OnPointerEnter(CursorPointingTypes cursorPointingTypes)
        {
            switch (cursorPointingTypes)
            {
                case CursorPointingTypes.Hover:
                    PlayScript(Configuration.HoverCursorSoundEnterNanicode);
                    break;
                case CursorPointingTypes.Interactive:
                    PlayScript(Configuration.InteractiveCursorSoundEnterNanicode);
                    break;
                case CursorPointingTypes.Examine:
                    PlayScript(Configuration.ExamineCursorSoundEnterNanicode);
                    break;
            }

            SetCursor(cursorPointingTypes);
        }

        public void OnPointerExit(CursorPointingTypes cursorType)
        {
            switch (cursorType)
            {
                case CursorPointingTypes.Hover:
                    PlayScript(Configuration.HoverCursorSoundExitNanicode);
                    break;
                case CursorPointingTypes.Interactive:
                    PlayScript(Configuration.InteractiveCursorSoundExitNanicode);
                    break;
                case CursorPointingTypes.Examine:
                    PlayScript(Configuration.ExamineCursorSoundExitNanicode);
                    break;
            }

            SetCursor(cursorType);
        }

        public void OnPointerDown(CursorPointingTypes cursorType)
        {
            switch (cursorType)
            {
                case CursorPointingTypes.Hover:
                    PlayScript(Configuration.HoverCursorSoundDownNanicode);
                    break;
                case CursorPointingTypes.Interactive:
                    PlayScript(Configuration.InteractiveCursorSoundDownNanicode);
                    break;
                case CursorPointingTypes.Examine:
                    PlayScript(Configuration.ExamineCursorSoundDownNanicode);
                    break;
            }

            SetCursor(cursorType);
        }

        public void OnPointerUp(CursorPointingTypes cursorType)
        {
            switch (cursorType)
            {
                case CursorPointingTypes.Hover:
                    PlayScript(Configuration.HoverCursorSoundUpNanicode);
                    break;
                case CursorPointingTypes.Interactive:
                    PlayScript(Configuration.InteractiveCursorSoundUpNanicode);
                    break;
                case CursorPointingTypes.Examine:
                    PlayScript(Configuration.ExamineCursorSoundUpNanicode);
                    break;
            }

            SetCursor(cursorType);
        }
    }
}
