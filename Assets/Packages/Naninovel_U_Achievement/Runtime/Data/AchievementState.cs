using System;
using System.Collections.Generic;

namespace Naninovel.U.Achievement
{
    [Serializable]
    public class AchievementState
    {
        public List<string> RegisterAchievementActivatedKeys;

        public AchievementState()
        {
            // Initialization Values
            RegisterAchievementActivatedKeys = new List<string>();
        }

        public AchievementState(AchievementState other)
        {
            // Load and set Data
            RegisterAchievementActivatedKeys = new List<string>(other.RegisterAchievementActivatedKeys);
        }
    }
}
