using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Naninovel.U.Puzzle
{
    [Serializable]
    internal class PuzzlePartsKit : MonoBehaviour
    {
        public string Name;
        [SerializeField] private RectTransform[] parts;
        [Space]
        [SerializeField] private Image collectedImage;
        [SerializeField] private CanvasGroup canvasGroup;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public Vector2[] GetPartsPositions()
        {
            Vector2[] positions = new Vector2[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                positions[i] = parts[i].anchoredPosition;
            }
            return positions;
        }

        public void SetPartsPositions(Vector2[] newPositions)
        {
            if (newPositions.Length != parts.Length)
            {
                Debug.LogWarning("Positions array length does not match parts array length.");
                return;
            }

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].anchoredPosition = newPositions[i];
            }
        }

        public int GetPartIndex(RectTransform rectTransform)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].GetHashCode() == rectTransform.GetHashCode())
                {
                    return i;
                }
            }

            Debug.LogError("PuzzlePartsKit: RectTransform not found in parts array!");
            return 0;
        }

        public RectTransform[] GetAllParts() { return parts; }
        public Image GetCollectedImage() { return collectedImage; }
        public CanvasGroup GetPartsCanvasGroup() { return canvasGroup; }
    }
}