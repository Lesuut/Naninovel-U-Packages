using System;
using UnityEngine;

namespace Naninovel.U.UIInteractionToolkit
{
    [Serializable]
    public class UIInteractionItem
    {
        public CursorPointingTypes Type = CursorPointingTypes.Hover;
        public Texture2D Cursor;
        public string SoundEnterNanicode;
        public string SoundExitNanicode;
        public string SoundDownNanicode;
        public string SoundUpNanicode;
    }
}
