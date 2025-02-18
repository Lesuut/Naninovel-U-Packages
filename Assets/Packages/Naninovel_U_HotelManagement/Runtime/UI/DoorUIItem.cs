using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.U.HotelManagement
{
    public class DoorUIItem : MonoBehaviour
    {
        public RectTransform rectTransform;

        [Space]
        [SerializeField] private Button button;
        [Space]
        [SerializeField] private UnityEvent OccupyDoorEvent;
        [SerializeField] private UnityEvent ReleaseDoorEvent;
        [SerializeField] private UnityEvent ReadyForOccupyDoorEvent;
        [Space]
        [SerializeField] private Image moodBar;
        [SerializeField] private Gradient moodBarColor;
        [Space]
        [SerializeField] private Image NotificationIcone;

        public void Init(Action buttonAction)
        {
            button.onClick.AddListener(() => buttonAction());
        }
        public void OccupyDoor()
        {
            OccupyDoorEvent?.Invoke();
        }
        public void ReleaseDoor()
        {
            ReleaseDoorEvent?.Invoke();
        }
        public void SetIcone(Sprite icone)
        {
            NotificationIcone.enabled = true;
            NotificationIcone.sprite = icone;
        }
        public void HideIcone()
        {
            NotificationIcone.enabled = false;
        }
        public void SetMood(float mood)
        {
            moodBar.fillAmount = mood;
            moodBar.color = moodBarColor.Evaluate(mood);
        }
    }
}
