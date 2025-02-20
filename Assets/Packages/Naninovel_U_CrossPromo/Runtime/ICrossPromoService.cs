using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    public interface ICrossPromoService : IEngineService
    {
        public void ShowCrossPromo();
        public void UnlockItem(int id);
        public void UnlockRandomItem();
    }
}