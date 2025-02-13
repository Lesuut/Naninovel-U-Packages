using System;
using System.Collections.Generic;

namespace Naninovel.U.Reception
{
    [Serializable]
    public class ReceptionState
    {
        public string scriptName;
        public int scriptPlayedIndex;

        public ReceptionState()
        {
            scriptName = "";
            scriptPlayedIndex = 0;
        }

        public ReceptionState(ReceptionState other)
        {
            scriptName= other.scriptName;
            scriptPlayedIndex= other.scriptPlayedIndex;
        }
    }
}
