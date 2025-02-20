using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Naninovel.U.CrossPromo
{
    public class SlotItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum ClickType
        {
            Instant,
            Delayed
        }

        [SerializeField] private ClickType clickType = ClickType.Instant;
        [SerializeField] private float holdTime = 1.0f;
        [Space]
        [SerializeField] private Image uploadedPictureImage;
        [Space]
        [SerializeField] private UnityEvent<float> onHoldProgress;
        [Space]
        [SerializeField] private UnityEvent changeStatusToReceived;
        [SerializeField] private UnityEvent changeStatusToPending;

        private UnityAction action;
        private Coroutine holdCoroutine;

        public void Initialize(UnityAction action, Sprite uploadedPicture)
        {
            this.action = action;
            uploadedPictureImage.sprite = uploadedPicture;
        }

        public void SetReceivedStatus(bool status)
        {
            if (status)
                changeStatusToReceived?.Invoke();
            else
                changeStatusToPending?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (clickType == ClickType.Instant)
            {
                action?.Invoke();
            }
            else if (clickType == ClickType.Delayed)
            {
                if (holdCoroutine != null)
                {
                    StopCoroutine(holdCoroutine);
                }
                holdCoroutine = StartCoroutine(HoldButton());
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (clickType == ClickType.Delayed && holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
                holdCoroutine = null;
                onHoldProgress?.Invoke(0f); // Сброс прогресса
            }
        }

        private IEnumerator HoldButton()
        {
            float elapsedTime = 0f;
            while (elapsedTime < holdTime)
            {
                elapsedTime += Time.deltaTime;
                onHoldProgress?.Invoke(elapsedTime / holdTime);
                yield return null;
            }
            action?.Invoke(); // Запуск действия, если удержание прошло успешно
            onHoldProgress?.Invoke(1f); // Полное заполнение
        }
    }
}
