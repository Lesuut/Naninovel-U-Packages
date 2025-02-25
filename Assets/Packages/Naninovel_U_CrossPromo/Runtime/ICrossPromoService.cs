using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    public interface ICrossPromoService : IEngineService
    {
        public void ShowCrossPromo(LinkTransitionType linkTransitionType);
        public void UnlockItem(int id);
        public void UnlockRandomItem();
        public bool IsCGSlotValid(string unlockableKey);
    }
}