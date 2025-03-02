using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    public interface IHotelManagementManager : IEngineService
    {
        public void StartMiniGame(string levelName);
        public void Improve(string key);
        public bool IsHotelWin();
    }
}