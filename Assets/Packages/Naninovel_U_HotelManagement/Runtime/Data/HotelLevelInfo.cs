using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    [CreateAssetMenu(fileName = "NewHotelLevelInfo", menuName = "Hotel/HotelLevelInfo", order = 1)]
    public class HotelLevelInfo : ScriptableObject
    {
        public int MiniGameTimeSeconds;
        [Space]
        public float MinGuestsSpawnTime;
        public float MaxGuestsSpawnTime;
        [Header("Mood")]
        [Range(0, 1)]
        public float MoodLossSpeed;
        [Range(0, 1)]
        public float MoodPlusValue;
        [Header("Food")]
        public int MinNumberFoodOrders;
        public int MaxNumberFoodOrders;
        [Space]
        public int MinTimeFoodOrders;
        public int MaxTimeFoodOrders;
        [Header("Food")]
        public int MinCliningTime;
        public int MaxCliningTime;
        [Space]
        public int MaxMoneyRevard = 25;
        [Space]
        public float MoveSpeed = 750;
    }
}
