using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Naninovel.U.UIInteractionToolkit
{
    public class InteractionUIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected CursorPointingTypes cursorPointingType = CursorPointingTypes.Hover;
        [Space]
        [SerializeField] protected bool useOnPointerDown = true;
        [SerializeField] protected bool useOnPointerUp = true;
        [SerializeField] protected bool useOnPointerEnter = true;
        [SerializeField] protected bool useOnPointerExit = true;
        [Space]
        [SerializeField] protected UnityEvent onPointerDown;
        [SerializeField] protected UnityEvent onPointerUp;
        [SerializeField] protected UnityEvent onPointerEnter;
        [SerializeField] protected UnityEvent onPointerExit;

        protected IUIInteractionToolkitManager interactionToolkitManager;
        private bool isPressed = false;

        private void OnEnable()
        {
            interactionToolkitManager = Engine.GetService<IUIInteractionToolkitManager>();
        }
      
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!useOnPointerDown) return;

            isPressed = true;
            interactionToolkitManager.OnPointerDown(cursorPointingType);

            onPointerDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!useOnPointerUp) return;

            if (isPressed)
            {
                isPressed = false;
                interactionToolkitManager.OnPointerUp(cursorPointingType);
            }

            onPointerUp?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!useOnPointerEnter) return;

            interactionToolkitManager.OnPointerEnter(cursorPointingType);

            onPointerEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!useOnPointerExit) return;

            interactionToolkitManager.OnPointerExit(cursorPointingType);

            onPointerExit?.Invoke();
        }
    }
}