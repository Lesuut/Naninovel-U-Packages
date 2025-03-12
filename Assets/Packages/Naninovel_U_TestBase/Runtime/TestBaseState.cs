using System;
using System.Collections.Generic;

namespace Naninovel.U.TestBase
{
    [Serializable]
    public class TestBaseState
    {
        public List<string> Value;

        public TestBaseState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public TestBaseState(TestBaseState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
