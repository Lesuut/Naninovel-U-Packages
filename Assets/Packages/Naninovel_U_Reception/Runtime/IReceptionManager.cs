using UnityEngine;

namespace Naninovel.U.Reception
{
    public interface IReceptionManager : IEngineService
    {
        public void PlayReceptionMiniGame(int cardCound);
        public bool IsReceptionWin();
    }
}