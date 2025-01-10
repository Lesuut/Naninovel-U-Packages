using System;
using System.Collections.Generic;

namespace Naninovel.U.MyOne
{
    [Serializable]
    public class MyOneState
    {
        public List<string> Value;

        public MyOneState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public MyOneState(MyOneState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
