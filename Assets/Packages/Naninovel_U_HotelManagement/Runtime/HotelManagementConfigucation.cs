using System.Linq;
using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    /// <summary>
    /// Contains configuration data for the HotelManagement systems.
    /// </summary>
    [EditInProjectSettings]
    public class HotelManagementConfiguration : Configuration
    {
        [System.Serializable]
        public struct MiniGameIcone
        {
            public MiniGameEventsType MiniGameEventsType;
            public Sprite icone;
        }

        public const string DefaultPathPrefix = "HotelManagement";

        [SerializeField] HotelLevelInfo[] levels;
        [Space]
        [SerializeField] private MiniGameIcone[] icones;
        [Space]
        public float actionTimeDuration = 4;

        public Sprite GetIcone(MiniGameEventsType miniGameEventsType) => icones.FirstOrDefault((item) => item.MiniGameEventsType == miniGameEventsType).icone;
        public HotelLevelInfo GetLevel(string name) => levels.FirstOrDefault((item) => item.name == name);
    }
}