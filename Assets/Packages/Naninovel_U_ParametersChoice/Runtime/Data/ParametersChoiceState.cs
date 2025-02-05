using System;
using System.Collections.Generic;

namespace Naninovel.U.ParametersChoice
{
    [Serializable]
    public class ParametersChoiceState
    {
        public List<string> Value;

        public ParametersChoiceState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public ParametersChoiceState(ParametersChoiceState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
