using Naninovel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.U.CrossPromo
{
    public class CrossPromoUI : CustomUI
    {
        public List<SlotItemUI> SlotItems { get; private set; } = new List<SlotItemUI>();

        [Header("Slot")]
        [SerializeField] private Transform slotPerent;
        [SerializeField] private GameObject slotPrefab;
        [Header("Adult")]
        [SerializeField] private GameObject adultWindowObj;
        [SerializeField] private CanvasGroup adultCanvasGroup;
        [SerializeField] private Image adultImage;
        [SerializeField] private Button adultButton;
        [SerializeField] private UnityEvent adultShowEvent;
        [SerializeField] private UnityEvent adultHideEvent;
        [Header("Continue")]
        [SerializeField] private GameObject continueWindowObj;
        [SerializeField] private CanvasGroup continueCanvasGroup;
        [SerializeField] private Button continueButton;
        [SerializeField] private UnityEvent continueShowEvent;
        [SerializeField] private UnityEvent continueHideEvent;
        [Space]
        [SerializeField] private float showTime = 1f;
        [SerializeField] private float hideTime = 0.5f;

        private List<GameObject> slotObls = new List<GameObject>();

        public void ShowAdult(Sprite sprite)
        {
            adultImage.sprite = sprite;
            adultButton.onClick.RemoveAllListeners();
            adultButton.onClick.AddListener(() =>
            {
                StartCoroutine(FadeCoroutine(adultCanvasGroup, adultWindowObj, false, hideTime));
                adultHideEvent?.Invoke();
            });

            adultShowEvent?.Invoke();

            // Устанавливаем начальную прозрачность в 0 перед анимацией
            adultCanvasGroup.alpha = 0f;
            StartCoroutine(FadeCoroutine(adultCanvasGroup, adultWindowObj, true, showTime));
        }

        public void ShowContinueWindow(Action action)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                StartCoroutine(FadeCoroutine(continueCanvasGroup, continueWindowObj, false, hideTime));
                continueHideEvent?.Invoke();
                action?.Invoke();
            });

            continueShowEvent?.Invoke();

            // Устанавливаем начальную прозрачность в 0 перед анимацией
            continueCanvasGroup.alpha = 0f;
            StartCoroutine(FadeCoroutine(continueCanvasGroup, continueWindowObj, true, showTime));
        }

        public override void Hide()
        {
            base.Hide();
            continueWindowObj.SetActive(false);
            adultWindowObj.SetActive(false);
        }

        public void OpenUrl(string url) => SteamUrlOpener.OpenUrl(url);

        public void SpawnSlot(Sprite uploadedPicture, UnityAction action, int ID)
        {
            GameObject obj = Instantiate(slotPrefab, slotPerent);
            SlotItemUI slotItemUI = obj.GetComponent<SlotItemUI>();

            slotItemUI.Initialize(action, uploadedPicture, ID);

            SlotItems.Add(slotItemUI);
            slotObls.Add(obj);
        }

        public void ClearSlots()
        {
            foreach (var item in slotObls)
            {
                Destroy(item);
            }
            slotObls.Clear();
            SlotItems.Clear();
        }

        private IEnumerator FadeCoroutine(CanvasGroup canvasGroup, GameObject targetObject, bool visibility, float duration)
        {
            float elapsedTime = 0f;
            float startAlpha = canvasGroup.alpha;
            float targetAlpha = visibility ? 1f : 0f;

            if (visibility && !targetObject.activeSelf)
            {
                targetObject.SetActive(true);
            }

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;

            if (!visibility)
            {
                targetObject.SetActive(false);
            }
        }
    }
}