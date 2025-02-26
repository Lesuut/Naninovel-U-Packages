using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.U.CG
{
    public class UCGGallerySlot : MonoBehaviour, IUpdatedCGSlot
    {
        [SerializeField] private string unlockableKey;
        [Space]
        [SerializeField] private Button button;
        [SerializeField] private UnityEvent openContent;
        [Space]
        [SerializeField] private UnityEvent resetStatus;
        [SerializeField] private UnityEvent unlockSlot;
        [SerializeField] private UnityEvent lockSlot;

        private IUnlockableManager unlockableManager;

        public void Init()
        {
            unlockableManager = Engine.GetService<IUnlockableManager>();
        }

        public void SlotUpdate()
        {
            if (unlockableManager.ItemUnlocked(unlockableKey))
            {

            }
        }
    }
}