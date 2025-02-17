using System;
using System.Collections.Generic;

namespace Naninovel.U.HotelManagement
{
    [Serializable]
    public class HotelManagementState
    {
        public List<string> Value;

        public HotelManagementState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public HotelManagementState(HotelManagementState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
