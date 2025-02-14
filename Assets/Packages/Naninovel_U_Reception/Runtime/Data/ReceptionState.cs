using System;
using System.Collections.Generic;

namespace Naninovel.U.Reception
{
    [Serializable]
    public class ReceptionState
    {
        public string scriptName;
        public int scriptPlayedIndex;

        public int correctAnswer;
        public int incorrectAnswer;

        public List<int> pairIdsChain;

        public ReceptionState()
        {
            scriptName = "";
            scriptPlayedIndex = 0;
            pairIdsChain = new List<int>();
        }

        public ReceptionState(ReceptionState other)
        {
            scriptName= other.scriptName;
            scriptPlayedIndex= other.scriptPlayedIndex;
            pairIdsChain = other.pairIdsChain;
            correctAnswer = other.correctAnswer;
            incorrectAnswer = other.incorrectAnswer;
        }
    }
}
