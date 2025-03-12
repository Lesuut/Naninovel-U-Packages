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
            button.onClick.AddListener(() => openContent?.Invoke());
        }

        public void SlotUpdate()
        {
            resetStatus?.Invoke();

            if (unlockableManager.ItemUnlocked(unlockableKey))
            {
                unlockSlot?.Invoke();
            }
            else
            {
                lockSlot?.Invoke();
            }
        }
    }
}