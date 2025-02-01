using System;
using System.Collections.Generic;

namespace Naninovel.U.Flow
{
    [Serializable]
    public class FlowState
    {
        public List<string> Value;

        public FlowState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public FlowState(FlowState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
