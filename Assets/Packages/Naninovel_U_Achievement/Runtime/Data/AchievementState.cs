using System;
using System.Collections.Generic;

namespace Naninovel.U.Achievement
{
    [Serializable]
    public class AchievementState
    {
        public List<string> Value;

        public AchievementState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public AchievementState(AchievementState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
