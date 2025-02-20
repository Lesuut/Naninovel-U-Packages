using System;
using System.Collections.Generic;

namespace Naninovel.U.CrossPromo
{
    [Serializable]
    public class CrossPromoState
    {
        public List<string> Value;

        public CrossPromoState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public CrossPromoState(CrossPromoState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
