using UnityEngine;
using UnityEngine.Events;

namespace Naninovel.U.CrossPromo.Commands
{
    public class CGSlotUI : MonoBehaviour
    {
        [SerializeField] private string unlockableKey;
        [Space]
        [SerializeField] private UnityEvent resetUnityEvent;
        [SerializeField] private UnityEvent lockUnityEvent;
        [SerializeField] private UnityEvent unlockUnityEvent;

        public void UpdateStatus()
        {
            resetUnityEvent?.Invoke();

            if (Engine.GetService<IUnlockableManager>().ItemUnlocked(unlockableKey))
                unlockUnityEvent?.Invoke();
            else
                lockUnityEvent?.Invoke();
        }
    }
}