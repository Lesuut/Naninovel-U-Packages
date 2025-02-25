using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.U.CrossPromo
{
    public class CrossPromoCGSlotUI : MonoBehaviour
    {
        [SerializeField] private string unlockableKey;
        [Space]
        [Tooltip("Вставьте сюда событие для открытия рекламы")]
        [SerializeField] private UnityEvent openAdultEventButton;
        [SerializeField] private UnityEvent hideCGGalleryEvent; // Важно назначить Hide окна вашей галереи UI для коректной работы.
        [Space]
        [SerializeField] private UnityEvent resetUnityEvent;
        [SerializeField] private UnityEvent lockUnityEvent;
        [SerializeField] private UnityEvent unlockUnityEvent;
        [Space]
        [SerializeField] private Button button;

        public void UpdateStatus()
        {
            // Если в инспекторе отключена галочка "Enable кросс-промо", то слот будет отключен
            if (!Engine.GetService<ICrossPromoService>().IsCrossPromoEnabled()) return;

            // Проверка на валидность. Например, если в галерее 9 слотов, а в таблице только 6 строк,
            // то метод проверит, входит ли данный слот в допустимый диапазон по кдючу unlockable.
            // В нашем случае метод отключит ненужные элементы галереи, оставив только 6 активных.
            // Рекомендуется изменить эту логику под вашу галерею.
            gameObject.SetActive(Engine.GetService<ICrossPromoService>().IsCGSlotValid(unlockableKey));

            resetUnityEvent?.Invoke();

            // Проверка, был ли элемент уже разблокирован
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
