using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    [InitializeAtRuntime()]
    public class UIInteractionToolkitManager : IUIInteractionToolkitManager
    {
        public virtual UIInteractionToolkitConfiguration Configuration { get; }

        private GameObject currentGameObject;

        public UIInteractionToolkitManager(UIInteractionToolkitConfiguration config)
        {
            Configuration = config;
        }
        public UniTask InitializeServiceAsync()
        {
            SetCursor(CursorPointingTypes.Defoult);
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
                case CursorPointingTypes.Action:
                    Cursor.SetCursor(Configuration.ActionCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(Configuration.DefoultCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }

        public void OnPointerEnter(CursorPointingTypes cursorPointingTypes)
        {
            if (currentGameObject != null) return;

            switch (cursorPointingTypes)
            {
                case CursorPointingTypes.Hover:
                    PlayScript(Configuration.HoverCursorSoundEnterNanicode);
                    break;
                case CursorPointingTypes.Interactive:
                    PlayScript(Configuration.InteractiveCursorSoundEnterNanicode);
                    break;
                case CursorPointingTypes.Action:
                    PlayScript(Configuration.ActionCursorSoundEnterNanicode);
                    break;
            }

            SetCursor(cursorPointingTypes);
        }

        public void OnPointerExit(CursorPointingTypes cursorType)
        {
            if (currentGameObject != null) return;

            switch (cursorType)
            {
                case CursorPointingTypes.Hover:
                    PlayScript(Configuration.HoverCursorSoundExitNanicode);
                    break;
                case CursorPointingTypes.Interactive:
                    PlayScript(Configuration.InteractiveCursorSoundExitNanicode);
                    break;
                case CursorPointingTypes.Action:
                    PlayScript(Configuration.ActionCursorSoundExitNanicode);
                    break;
            }

            SetCursor(CursorPointingTypes.Defoult);
        }

        public void OnPointerDown(CursorPointingTypes cursorType, bool useCatch, GameObject gameObject)
        {
            if (useCatch)
            {
                currentGameObject = gameObject;
                Cursor.SetCursor(Configuration.CatchCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        public void OnPointerUp(CursorPointingTypes cursorType, GameObject gameObject)
        {
            if (currentGameObject != null && currentGameObject.GetHashCode() != gameObject.GetHashCode())
                return;

            SetCursor(CursorPointingTypes.Defoult);

            currentGameObject = null;
        }
    }
}
