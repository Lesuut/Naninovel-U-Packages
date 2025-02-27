using System;
using System.Collections.Generic;

namespace Naninovel.U.Reception
{
    [Serializable]
    public class ReceptionState
    {
        public int scriptPlayedIndex;

        public int correctAnswer;
        public int incorrectAnswer;

        public List<int> pairIdsChain;

        public ReceptionState()
        {
            scriptPlayedIndex = 0;
            pairIdsChain = new List<int>();
        }

        public ReceptionState(ReceptionState other)
        {
            scriptPlayedIndex= other.scriptPlayedIndex;
            pairIdsChain = other.pairIdsChain;
            correctAnswer = other.correctAnswer;
            incorrectAnswer = other.incorrectAnswer;
        }
    }
}
