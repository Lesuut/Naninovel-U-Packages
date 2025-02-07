using System;
using System.Collections.Generic;

namespace Naninovel.U.ChoiceBox
{
    [Serializable]
    public class ChoiceBoxState
    {
        public bool isChoiceBoxActive;
        public string currentChoiceTitle;
        public List<ChoiceBoxItem> choiceItem;

        public string startScriptName;
        public int startScriptPlayedIndex;

        public ChoiceBoxState()
        {
            isChoiceBoxActive = false;
            currentChoiceTitle = "";
            choiceItem = new List<ChoiceBoxItem>();
        }

        public ChoiceBoxState(ChoiceBoxState other)
        {
            isChoiceBoxActive = other.isChoiceBoxActive;
            currentChoiceTitle = other.currentChoiceTitle;
            choiceItem = other.choiceItem;
            startScriptName = other.startScriptName;
            startScriptPlayedIndex = other.startScriptPlayedIndex;
        }
    }
}
