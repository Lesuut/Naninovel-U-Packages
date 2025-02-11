using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.U.UIInteractionToolkit
{
    public class InteractionUIElementSmartCollider : InteractionUIElement, ICanvasRaycastFilter
    {
        [Space]
        [SerializeField] private bool smartCollider = true;

        private Image image;

        protected override void OnEnable()
        {
            base.OnEnable();

            TryGetComponent<Image>(out image);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (smartCollider && image != null && image.sprite != null)
            {
                // Получаем текстуру спрайта
                Texture2D texture = image.sprite.texture;

                // Получаем размеры и позицию RectTransform изображения
                RectTransform rectTransform = image.rectTransform;
                Vector2 size = rectTransform.rect.size;
                Vector2 position = rectTransform.position;

                // Переводим мировые координаты в локальные
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint);

                // Преобразуем локальные координаты в координаты текстуры с учетом скейла
                Vector2 normalizedPos = new Vector2((localPoint.x + size.x * 0.5f) / size.x, (localPoint.y + size.y * 0.5f) / size.y);

                // Преобразуем в координаты текстуры (с учетом масштабирования)
                Vector2 textureCoords = new Vector2(normalizedPos.x * texture.width, normalizedPos.y * texture.height);

                // Проверяем, если координаты находятся в пределах текстуры
                if (textureCoords.x < 0 || textureCoords.x >= texture.width || textureCoords.y < 0 || textureCoords.y >= texture.height)
                    return false;

                // Получаем цвет пикселя
                Color pixelColor = texture.GetPixel((int)textureCoords.x, (int)textureCoords.y);

                // Если альфа-канал пикселя равен нулю, считаем его прозрачным
                return pixelColor.a > 0;
            }

            return false;
        }
    }
}
