using System;
using System.Collections.Generic;
using System.Linq;

namespace Naninovel.U.Parameters
{
    [Serializable]
    public class ParametersState
    {
        public List<ParameterItem> Parameters;

        public ParametersState()
        {
            // Initialization Values
            Parameters = new List<ParameterItem>
            {
               new ParameterItem()
               {
                   Key = "Reputation",
                   Value = 0
               },
               new ParameterItem()
               {
                   Key = "Profit",
                   Value = 0
               },
               new ParameterItem()
               {
                   Key = "Quality",
                   Value = 0
               },
               new ParameterItem()
               {
                   Key = "Personnel",
                   Value = 0
               }
            };
        }

        public ParametersState(ParametersState other)
        {
            // Load and set Data
            Parameters = new List<ParameterItem>(other.Parameters);
        }

        public void ResetAllParameters()
        {
            Parameters.ForEach(item => item.Value = 0);
        }
    }
}
