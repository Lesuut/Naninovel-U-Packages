using System;
using System.Collections.Generic;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public class SmartQuestState
    {
        public List<string> Value;

        public SmartQuestState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public SmartQuestState(SmartQuestState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
