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
            Delayed,
            Combined,
        }

        public int ID { get; private set; }

        [SerializeField] private ClickType clickType = ClickType.Instant;
        [SerializeField] private float holdTime = 1.0f;
        [SerializeField] private Button button;
        [Space]
        [SerializeField] private Image uploadedPictureImage;
        [Space]
        [SerializeField] private UnityEvent<float> onHoldProgress;
        [Space]
        [SerializeField] private UnityEvent changeStatusToReceived;
        [SerializeField] private UnityEvent changeStatusToPending;

        private UnityAction action;
        private Coroutine holdCoroutine;
        private bool receivedStatus;

        public void Initialize(UnityAction action, Sprite uploadedPicture, int ID)
        {
            this.ID = ID;
            this.action = action;
            uploadedPictureImage.sprite = uploadedPicture;
            receivedStatus = false;

            if (clickType == ClickType.Instant)
                button.onClick.AddListener(() => action?.Invoke());
        }

        public void SetReceivedStatus(bool status)
        {
            onHoldProgress?.Invoke(0);
            receivedStatus = status;

            if (status)
                changeStatusToReceived?.Invoke();
            else
                changeStatusToPending?.Invoke();

            if (receivedStatus && clickType == ClickType.Combined)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(action);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (clickType)
            {
                case ClickType.Delayed:
                    StartHoldCoroutine();
                    break;

                case ClickType.Combined:
                    if (!receivedStatus)
                        StartHoldCoroutine();
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if ((clickType == ClickType.Delayed || (clickType == ClickType.Combined)) && holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
                holdCoroutine = null;
                onHoldProgress?.Invoke(0f); // Сброс прогресса
            }
        }

        private void StartHoldCoroutine()
        {
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
            }
            holdCoroutine = StartCoroutine(HoldButton());
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
