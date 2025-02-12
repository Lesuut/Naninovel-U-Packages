using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    [InitializeAtRuntime()]
    public class UIInteractionToolkitManager : IUIInteractionToolkitManager
    {
        public virtual UIInteractionToolkitConfiguration Configuration { get; }

        private Texture2D lastCursor;
        private bool nowButtonPress;

        public UIInteractionToolkitManager(UIInteractionToolkitConfiguration config)
        {
            Configuration = config;
            nowButtonPress = false;
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
            nowButtonPress = false;
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
            if (cursorPointingTypes == CursorPointingTypes.Defoult)
            {
                Cursor.SetCursor(Configuration.DefoultCursor, Vector2.zero, CursorMode.Auto);
                lastCursor = Configuration.DefoultCursor;
            }
            else
            {
                var item = Configuration.UIInteractionItems.FirstOrDefault(item => item.Type == cursorPointingTypes);
                Cursor.SetCursor(item.Cursor, Vector2.zero, CursorMode.Auto);
                lastCursor = item.Cursor;
            }
        }

        public void OnPointerEnter(CursorPointingTypes cursorPointingTypes)
        {
            if (!nowButtonPress)
            {
                if (cursorPointingTypes != CursorPointingTypes.Defoult)
                {
                    var item = Configuration.UIInteractionItems.FirstOrDefault(item => item.Type == cursorPointingTypes);
                    PlayScript(item.SoundEnterNanicode);
                }

                SetCursor(cursorPointingTypes);
            }
        }

        public void OnPointerExit(CursorPointingTypes cursorPointingTypes)
        {
            if (!nowButtonPress)
            {
                if (cursorPointingTypes != CursorPointingTypes.Defoult)
                {
                    var item = Configuration.UIInteractionItems.FirstOrDefault(item => item.Type == cursorPointingTypes);
                    PlayScript(item.SoundExitNanicode);
                }

                SetCursor(CursorPointingTypes.Defoult);
            }
        }

        public void OnPointerDown(CursorPointingTypes cursorPointingTypes, bool useCatch)
        {
            nowButtonPress = true;

            if (useCatch)
            {
                Cursor.SetCursor(Configuration.CatchCursor, Vector2.zero, CursorMode.Auto);
            }

            if (cursorPointingTypes != CursorPointingTypes.Defoult)
            {
                var item = Configuration.UIInteractionItems.FirstOrDefault(item => item.Type == cursorPointingTypes);
                PlayScript(item.SoundDownNanicode);
            }
        }

        public void OnPointerUp(CursorPointingTypes cursorPointingTypes)
        {
            nowButtonPress = false;

            if (cursorPointingTypes != CursorPointingTypes.Defoult)
            {
                var item = Configuration.UIInteractionItems.FirstOrDefault(item => item.Type == cursorPointingTypes);
                PlayScript(item.SoundUpNanicode);
            }

            if (lastCursor != null)
                Cursor.SetCursor(lastCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
