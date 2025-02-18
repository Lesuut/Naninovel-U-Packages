using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.U.HotelManagement
{
    public class HotelManagementUI : CustomUI
    {
        public HotelMovementSystem hotelMovementSystem;
        public DoorUIItem[] doorUIItems;
        [Space]
        public Button ButtonReception;
        public Button ButtonFood;
        public Button ButtonCleaning;
        [Space]
        public RectTransform player;
        [SerializeField] private Image ReceptionKey;

        public void ShowKey()
        {
            ReceptionKey.enabled = true;
        }
        public void HideKey() 
        {
            ReceptionKey.enabled = false;
        }
    }
}