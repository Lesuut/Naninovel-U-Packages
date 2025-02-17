using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.U.HotelManagement
{
    public class DoorItem : MonoBehaviour
    {
        public Button button;
        [Space]
        [SerializeField] private UnityEvent OccupyDoorEvent;
        [SerializeField] private UnityEvent ReleaseDoorEvent;
        [Space]
        [SerializeField] private Image moodBar;
        [SerializeField] private Gradient moodBarColor;
        [Space]
        [SerializeField] private Image NotificationIcone;

        public void Init(Action<DoorItem> buttonAction)
        {
            button.onClick.AddListener(() => buttonAction(this));
        }

        public void OccupyDoor()
        {
            OccupyDoorEvent?.Invoke();
        }
        public void ReleaseDoor()
        {
            ReleaseDoorEvent?.Invoke();
        }
    }
}
