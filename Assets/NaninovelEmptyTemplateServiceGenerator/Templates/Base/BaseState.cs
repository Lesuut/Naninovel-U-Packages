using System;
using System.Collections.Generic;

namespace Naninovel.U.Base
{
    [Serializable]
    public class BaseState
    {
        public List<string> Value;

        public BaseState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public BaseState(BaseState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
