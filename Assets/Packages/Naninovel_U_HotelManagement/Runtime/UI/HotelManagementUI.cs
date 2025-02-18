using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.U.HotelManagement
{
    public class HotelManagementUI : CustomUI
    {
        [SerializeField] private HotelMovementSystem hotelMovementSystem;
        [SerializeField] private DoorUIItem[] doorUIItems;
        [Space]
        [SerializeField] private Button buttonReception;
        [SerializeField] private Button buttonFood;
        [SerializeField] private Button buttonCleaning;
        [Header("Player")]
        [SerializeField] private RectTransform playerRectTransform;
        [SerializeField] private Image playerHandItem;
        [Header("Stantions")]
        [SerializeField] private Image[] receptionKeysImage;
        [Space]
        [SerializeField] private Image[] receptionUpgrades;
        [SerializeField] private Image[] foodUpgrades;
        [SerializeField] private Image[] cleaningUpgrades;

        public HotelMovementSystem HotelMovementSystem { get => hotelMovementSystem; }
        public DoorUIItem[] DoorUIItems { get => doorUIItems; }
        public Button ButtonReception { get => buttonReception; }
        public Button ButtonFood { get => buttonFood; }
        public Button ButtonCleaning { get => buttonCleaning; }
        public RectTransform PlayerRectTransform { get => playerRectTransform; }
      
        public void SetPlayerItem(Sprite sprite)
        {
            playerHandItem.enabled = true;
            playerHandItem.sprite = sprite;
        }
        public void HidePlayerItem()
        {
            playerHandItem.enabled = false;
        }
        public void SetReceptionKeyView(int count)
        {
            count = Mathf.Clamp(count, 0, receptionKeysImage.Length);

            foreach (var item in receptionKeysImage)
                item.enabled = false;

            for (int i = 0; i < count; i++)
            {
                receptionKeysImage[i].enabled = true;
            }
        }
        public void SetreceptionUpgrade(int id)
        {
            foreach (var item in receptionUpgrades)
                item.enabled = false;

            receptionUpgrades[id].enabled = true;
        }
        public void FoodUpgrade(int id)
        {
            foreach (var item in foodUpgrades)
                item.enabled = false;

            foodUpgrades[id].enabled = true;
        }
        public void CleaningUpgrade(int id)
        {
            foreach (var item in cleaningUpgrades)
                item.enabled = false;

            cleaningUpgrades[id].enabled = true;
        }
    }
}
