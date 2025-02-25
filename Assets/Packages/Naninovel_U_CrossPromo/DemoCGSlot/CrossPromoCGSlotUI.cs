using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.U.CrossPromo
{
    public class CrossPromoCGSlotUI : MonoBehaviour
    {
        [SerializeField] private string unlockableKey;
        [Space]
        [Tooltip("Throw in an adalta opening here")]
        [SerializeField] private UnityEvent openAdultEventButton;
        [SerializeField] private UnityEvent hideCGGalleryEvent;
        [Space]
        [SerializeField] private UnityEvent resetUnityEvent;
        [SerializeField] private UnityEvent lockUnityEvent;
        [SerializeField] private UnityEvent unlockUnityEvent;
        [Space]
        [SerializeField] private Button button;

        public void UpdateStatus()
        {
            if (!Engine.GetService<ICrossPromoService>().IsCrossPromoEnabled()) return;

            gameObject.SetActive(Engine.GetService<ICrossPromoService>().IsCGSlotValid(unlockableKey)); // Если в табличке нет нужного елемента по айди, то отключаем

            resetUnityEvent?.Invoke();

            bool itemUnlocked = Engine.GetService<IUnlockableManager>().ItemUnlocked(unlockableKey);

            if (itemUnlocked)
            {
                unlockUnityEvent?.Invoke();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => openAdultEventButton?.Invoke());
            }
            else
            {
                lockUnityEvent?.Invoke();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    Engine.GetService<ICrossPromoService>().ShowCrossPromo(LinkTransitionType.Gallery);
                    hideCGGalleryEvent?.Invoke();
                });
            }
        }
    }
}