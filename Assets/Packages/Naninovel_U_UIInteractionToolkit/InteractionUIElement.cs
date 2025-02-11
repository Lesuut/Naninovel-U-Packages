using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Naninovel.U.UIInteractionToolkit
{
    public class InteractionUIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected CursorPointingTypes cursorPointingType = CursorPointingTypes.Hover;
        [Space]
        [SerializeField] protected bool useHover = true;
        [SerializeField] protected bool useCatch = false;
        [Space]
        [SerializeField] protected UnityEvent onPointerDown;
        [SerializeField] protected UnityEvent onPointerUp;
        [SerializeField] protected UnityEvent onPointerEnter;
        [SerializeField] protected UnityEvent onPointerExit;

        protected IUIInteractionToolkitManager interactionToolkitManager;
        private bool isPressed = false;

        protected virtual void OnEnable()
        {
            interactionToolkitManager = Engine.GetService<IUIInteractionToolkitManager>();
        }
      
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke();

            isPressed = true;
            interactionToolkitManager.OnPointerDown(cursorPointingType, useCatch, gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke();

            if (isPressed)
            {
                isPressed = false;
                interactionToolkitManager.OnPointerUp(cursorPointingType, gameObject);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke();

            if (!useHover) return;

            interactionToolkitManager.OnPointerEnter(cursorPointingType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke();

            interactionToolkitManager.OnPointerExit(cursorPointingType);
        }      
    }
}