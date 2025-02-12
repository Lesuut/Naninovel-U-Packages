using System;
using System.Collections.Generic;

namespace Naninovel.U.Reception
{
    [Serializable]
    public class ReceptionState
    {
        public List<string> Value;

        public ReceptionState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public ReceptionState(ReceptionState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
