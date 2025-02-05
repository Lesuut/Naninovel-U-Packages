using UnityEngine;
using Naninovel.UI;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Naninovel.U.Flow
{
    public class FlowUI : CustomUI
    {
        private Dictionary<string, TransitionButtonUI> buttons;
        private List<string> currentHideButtonKeys;

        public override UniTask InitializeAsync()
        {
            buttons = new Dictionary<string, TransitionButtonUI>();
            currentHideButtonKeys = new List<string>();
            return UniTask.CompletedTask;
        }

        public void CreateTransitionButton(GameObject prefab, UnityAction action)
        {
            // Проверяем, если кнопка с таким ключом уже существует
            if (buttons.ContainsKey(prefab.name))
            {
                // Получаем текущую кнопку и удаляем старые слушатели
                TransitionButtonUI existingButton = buttons[prefab.name];

                // Удаляем все слушатели с кнопки
                existingButton.TransitionButton.onClick.RemoveAllListeners();

                // Добавляем новый слушатель
                existingButton.TransitionButton.onClick.AddListener(action);

                existingButton.Show();

                return; // Выходим из метода, так как новая кнопка не требуется
            }

            TransitionButtonUI transitionButtonUI = Instantiate(prefab, transform.parent).GetComponent<TransitionButtonUI>();
            transitionButtonUI.Show();

            // Сохраняем локальные позиции и масштаб
            Vector2 originalAnchoredPosition = transitionButtonUI.RectTransform.anchoredPosition;
            Vector2 originalLocalScale = transitionButtonUI.RectTransform.localScale;

            // Устанавливаем родителя без изменения мировых координат
            transitionButtonUI.RectTransform.SetParent(transform, false);

            // Восстанавливаем локальные координаты с учетом нового масштаба
            transitionButtonUI.RectTransform.localScale = originalLocalScale;
            transitionButtonUI.RectTransform.anchoredPosition = originalAnchoredPosition;

            transitionButtonUI.TransitionButton.onClick.AddListener(action);

            // Сохраняем ссылку на новую кнопку в словарь
            buttons[prefab.name] = transitionButtonUI;
        }
        public void HideAllButtons()
        {
            if (buttons.Count == 0)
                return;

            foreach (var item in buttons)
            {
                if (item.Value != null && item.Value.Visible)
                {
                    item.Value.Hide();
                }
            }
        }
        public void SetHideButtonsStatus(bool hideStatus)
        {
            if (hideStatus)
            {
                foreach (var item in buttons)
                {
                    if (item.Value.Visible)
                    {
                        currentHideButtonKeys.Add(item.Key);
                        item.Value.Hide();
                    }
                }
            }
            else
            {
                currentHideButtonKeys.ForEach(item => buttons[item].Show());
                currentHideButtonKeys.Clear();
            }
        }
    }
}