using UnityEngine;
using Naninovel.UI;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

namespace Naninovel.U.Reception
{
    public class ReceptionUI : CustomUI
    {
        [SerializeField] private Text screenText;
        [SerializeField] private Text cardText;
        [Space]
        [SerializeField] private Text authorPrinter;
        [SerializeField] private Text textPrinter;
        [Space]
        [SerializeField] private Image cardImage;
        [SerializeField] private Gradient gradient;
        [Space]
        [SerializeField] private RectTransform cardRectTransform;
        [SerializeField] private RectTransform keyRectTransform;
        [Space]
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;
        [Header("Anim")]
        [SerializeField] private float time = 1;
        [SerializeField] private float yPosMoveCard = 800;
        [SerializeField] private float startYPosMoveKey;
        [SerializeField] private float endYPosMoveKey;

        private UnityAction currentendChoiceAction;
        private Coroutine coroutinePrinter;

        public void ShowPair(string screenText, string cardText, UnityAction acceptButton, UnityAction cancelButton, UnityAction endChoiceAction)
        {
            currentendChoiceAction = endChoiceAction;

            StartCoroutine(TypeText(this.screenText, screenText, 0.5f));
            this.cardText.text = cardText;

            this.acceptButton.onClick.RemoveAllListeners();
            this.acceptButton.onClick.AddListener(acceptButton);

            this.cancelButton.onClick.RemoveAllListeners();
            this.cancelButton.onClick.AddListener(cancelButton);

            cardImage.color = gradient.Evaluate(Random.Range(0, 1f));
            cardRectTransform.anchoredPosition = new Vector2(cardRectTransform.anchoredPosition.x, 800);
            cardRectTransform.DOAnchorPos(new Vector2(cardRectTransform.anchoredPosition.x, 0), time / 2);
        }

        public void AcceptAnim()
        {
            cardRectTransform.DOAnchorPos(new Vector2(cardRectTransform.anchoredPosition.x, 800), time)
                .OnComplete(() => currentendChoiceAction?.Invoke());

            keyRectTransform.anchoredPosition = new Vector2(keyRectTransform.anchoredPosition.x, startYPosMoveKey);
            keyRectTransform.DOAnchorPos(new Vector2(keyRectTransform.anchoredPosition.x, endYPosMoveKey), time);
        }

        public void CancelAnim()
        {
            cardRectTransform.DOAnchorPos(new Vector2(cardRectTransform.anchoredPosition.x, 800), time)
                .OnComplete(() => currentendChoiceAction?.Invoke());
        }

        private IEnumerator TypeText(Text textComponent, string fullText, float duration)
        {
            textComponent.text = "";
            float delay = duration / fullText.Length;

            foreach (char letter in fullText)
            {
                textComponent.text += letter;
                yield return new WaitForSeconds(delay);
            }
        }

        public void Printer(string author, string text)
        {
            authorPrinter.text = author;
            if (coroutinePrinter != null) StopCoroutine(coroutinePrinter);
            coroutinePrinter = StartCoroutine(TypeText(textPrinter, text, 1.5f));
        }
    }
}