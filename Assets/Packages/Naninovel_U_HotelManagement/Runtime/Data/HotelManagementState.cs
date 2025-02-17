using System;

namespace Naninovel.U.HotelManagement
{
    [Serializable]
    public class HotelManagementState
    {
        public int ComplexityId;

        public bool ReceptionImproving;
        public bool FoodImproving;
        public bool CleanImproving;

        public string ScriptName;
        public int ScriptPlayedIndex;

        public HotelManagementState()
        {
            ComplexityId = 0;
            ReceptionImproving = false;
            FoodImproving = false;
            CleanImproving = false;
            ScriptName = string.Empty;
            ScriptPlayedIndex = 0;
        }

        public HotelManagementState(HotelManagementState other)
        {
            ComplexityId= other.ComplexityId;
            ReceptionImproving = other.ReceptionImproving;
            FoodImproving = other.FoodImproving;
            CleanImproving = other.CleanImproving;
            ScriptName = other.ScriptName;
            ScriptPlayedIndex= other.ScriptPlayedIndex;
        }
    }
}
