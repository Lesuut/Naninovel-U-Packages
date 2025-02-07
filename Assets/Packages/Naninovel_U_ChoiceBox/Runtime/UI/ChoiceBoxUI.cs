using UnityEngine;
using UnityEngine.UI;
using Naninovel.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;
using System.Xml.Schema;

namespace Naninovel.U.ChoiceBox
{
    public class ChoiceBoxUI : CustomUI
    {
        private class ChoiceButtonItem
        {
            public GameObject Obj;
            public Button Button;
            public TextMeshProUGUI Text;
        }

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private RectTransform choiceParentTrans;
        [SerializeField] private GameObject choicePrefab;

        private List<ChoiceButtonItem> choiceButtonItems;

        protected override void Awake()
        {
            base.Awake();
            choiceButtonItems = new List<ChoiceButtonItem>();
        }

        public void SetTitle(string title) => titleText.text = title;

        public void CreateChoice(string title, UnityAction action)
        {
            // Проверяем, есть ли неактивная кнопка
            ChoiceButtonItem choiceButtonItem = choiceButtonItems.Find(item => !item.Obj.activeSelf);

            if (choiceButtonItem == null) // Если нет свободной кнопки, создаем новую
            {
                GameObject newChoiceButtonObj = Instantiate(choicePrefab, choiceParentTrans);

                choiceButtonItem = new ChoiceButtonItem
                {
                    Obj = newChoiceButtonObj,
                    Button = newChoiceButtonObj.GetComponent<Button>(),
                    Text = newChoiceButtonObj.GetComponentInChildren<TextMeshProUGUI>()
                };

                choiceButtonItems.Add(choiceButtonItem);
            }

            // Активируем кнопку и задаем текст
            choiceButtonItem.Obj.SetActive(true);
            choiceButtonItem.Text.SetText(title);

            // Очищаем старые подписки, добавляем новую
            choiceButtonItem.Button.onClick.RemoveAllListeners();
            choiceButtonItem.Button.onClick.AddListener(action);
        }

        public void ClearChoices()
        {
            foreach (var item in choiceButtonItems)
            {
                item.Obj.SetActive(false);
            }
        }

        public void DestroyAllChoices()
        {
            foreach (var item in choiceButtonItems)
            {
                Destroy(item.Obj);
            }
            choiceButtonItems.Clear();
        }
        public override void Show()
        {
            base.Show();

            StartCoroutine(curUpdateGroups());

            // Задаем начальные значения
            choiceParentTrans.anchoredPosition = new Vector2(choiceParentTrans.anchoredPosition.x, -1080);
            choiceParentTrans.localScale = new Vector3(0.5f, 0.5f, 1f); // Начальный масштаб (меньше 1)

            // Анимация: смещаем вниз и увеличиваем масштаб
            Sequence sequence = DOTween.Sequence();
            sequence.Append(choiceParentTrans.DOAnchorPosY(0, 1f)) // Смещение вниз
                    .Join(choiceParentTrans.DOScale(new Vector3(1.2f, 1.2f, 1f), 1f)) // Увеличение масштаба до 1.2
                    .Append(choiceParentTrans.DOScale(new Vector3(1f, 1f, 1f), 0.2f)) // Немного уменьшить до 1
                    .SetEase(Ease.OutBack); // Установить плавное замедление в конце анимации
        }
        private IEnumerator curUpdateGroups() // Ну хоть ты его по лбу стукни не обновляеться, только такой костыль работаеть (и да, 1 раз включить выключить не рабоатет)
        {
            float time = 0.025f;
            for (int i = 0; i < 2; i++)
            {
                choiceParentTrans.gameObject.SetActive(false);
                yield return new WaitForSeconds(time);
                choiceParentTrans.gameObject.SetActive(true);
                yield return new WaitForSeconds(time);
            }
        }
    }
}
