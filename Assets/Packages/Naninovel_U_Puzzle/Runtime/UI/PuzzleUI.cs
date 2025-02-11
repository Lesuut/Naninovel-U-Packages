using UnityEngine;
using UnityEngine.UI;
using Naninovel.UI;
using System;

namespace Naninovel.U.Puzzle
{
    public class PuzzleUI : CustomUI
    {
        [SerializeField] private Canvas canvas;

        private RectTransform currentSelectPiceRectTransform;
        private Vector2 offset; // Смещение между позицией мыши и объектом

        [Serializable]
        private new class GameState
        {
            public string Value;
        }

        private void Update()
        {
            if (currentSelectPiceRectTransform == null || canvas == null) return;

            Vector2 mousePosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            // Учитываем смещение
            currentSelectPiceRectTransform.anchoredPosition = localPoint - offset;
        }

        public void SelectPice(RectTransform rectTransform)
        {
            currentSelectPiceRectTransform = rectTransform;

            // Сохраняем смещение в момент захвата
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            offset = localPoint - currentSelectPiceRectTransform.anchoredPosition;
        }

        public void UnselectPice()
        {
            currentSelectPiceRectTransform = null;
        }
    }
}
