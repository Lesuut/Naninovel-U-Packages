using System;
using System.Collections.Generic;

namespace Naninovel.U.HotelManagement
{
    [Serializable]
    public class HotelManagementState
    {
        public bool GameActive;

        public string LevelKey;

        public int ReceptionImproving;
        public int FoodImproving;
        public int CleanImproving;

        public int ScriptPlayedIndex;

        public List<float> CompletedMoods;

        public HotelManagementState()
        {
            GameActive = false;
            LevelKey = "";
            ReceptionImproving = 0;
            FoodImproving = 0;
            CleanImproving = 0;
            ScriptPlayedIndex = 0;
            CompletedMoods = new List<float>();
        }

        public HotelManagementState(HotelManagementState other)
        {
            GameActive = other.GameActive;
            LevelKey = other.LevelKey;
            ReceptionImproving = other.ReceptionImproving;
            FoodImproving = other.FoodImproving;
            CleanImproving = other.CleanImproving;
            ScriptPlayedIndex= other.ScriptPlayedIndex;
            CompletedMoods = other.CompletedMoods;
        }
    }
}
