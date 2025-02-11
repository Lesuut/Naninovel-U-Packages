using UnityEngine;

public class TestInput : MonoBehaviour
{
    // Ссылка на RectTransform объекта, который будет следовать за мышкой
    public RectTransform uiObject;

    // Ссылка на Canvas, к которому принадлежит uiObject
    public Canvas canvas;

    void Update()
    {
        if (uiObject == null || canvas == null) return;

        // Получаем позицию мыши в экранных координатах
        Vector2 mousePosition = Input.mousePosition;

        // Преобразуем экранные координаты мыши в локальные координаты Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, // Родительский RectTransform (Canvas)
            mousePosition,                    // Позиция мыши в экранных координатах
            canvas.worldCamera,               // Камера, используемая для рендеринга Canvas
            out Vector2 localPoint            // Выходная позиция в локальных координатах
        );

        // Устанавливаем anchorPosition объекта в локальные координаты
        uiObject.anchoredPosition = localPoint;
    }
}