using AssetKits.ParticleImage;
using Naninovel.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
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
        [Space]
        [SerializeField] private UnityEvent<float> receptionProgressBar;
        [SerializeField] private UnityEvent<float> foodProgressBar;
        [SerializeField] private UnityEvent<float> cleaningProgressBar;
        [Space]
        [SerializeField] private string moneySymbol = "$";
        [SerializeField] private Text moneyText;
        [SerializeField] private ParticleImage particleImage;
        [Space]
        [SerializeField] private Text timerText;

        public HotelMovementSystem HotelMovementSystem { get => hotelMovementSystem; }
        public DoorUIItem[] DoorUIItems { get => doorUIItems; }
        public Button ButtonReception { get => buttonReception; }
        public Button ButtonFood { get => buttonFood; }
        public Button ButtonCleaning { get => buttonCleaning; }
        public RectTransform PlayerRectTransform { get => playerRectTransform; }
        public UnityEvent<float> ReceptionProgressBar { get => receptionProgressBar; }
        public UnityEvent<float> FoodProgressBar { get => foodProgressBar; }
        public UnityEvent<float> CleaningProgressBar { get => cleaningProgressBar; }

        private int currentMoney = 0;
        private int moneyInQueue = 0;

        private Coroutine coroutineTimer;

        public void ResetUI()
        {
            ReceptionProgressBar?.Invoke(0);
            FoodProgressBar?.Invoke(0);
            CleaningProgressBar?.Invoke(0);

            currentMoney = 0;
            moneyInQueue = 0;
            particleImage.Stop();

            StartCoroutine(curMoneyAdd());

            particleImage.onFirstParticleFinish.RemoveAllListeners();
            particleImage.onFirstParticleFinish.AddListener(() => StartCoroutine(curMoneyAdd()));

            if (coroutineTimer != null)
                StopCoroutine(coroutineTimer);
        }

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
        public void SetReceptionUpgrade(int id)
        {
            foreach (var item in receptionUpgrades)
            {
                item.enabled = false;
            }

            receptionUpgrades[id].enabled = true;
        }
        public void SetFoodUpgrade(int id)
        {
            foreach (var item in foodUpgrades)
                item.enabled = false;

            foodUpgrades[id].enabled = true;
        }
        public void SetCleaningUpgrade(int id)
        {
            foreach (var item in cleaningUpgrades)
                item.enabled = false;

            cleaningUpgrades[id].enabled = true;
        }
        public void UpdateMoney(int money, RectTransform moneySpawnPoint)
        {
            moneyInQueue += money;
            particleImage.rectTransform.anchoredPosition = moneySpawnPoint.anchoredPosition;
            particleImage.rateOverTime = money;
            particleImage.Play();
        }
        public void StartTimer(int durationInSeconds, System.Action onComplete)
        {
            coroutineTimer = StartCoroutine(TimerCoroutine(durationInSeconds, onComplete));
        }
        private IEnumerator curMoneyAdd()
        {
            while (moneyInQueue > 0)
            {
                moneyInQueue -= 1;
                currentMoney++;
                moneyText.text = $"{currentMoney}{moneySymbol}";
                yield return new WaitForSeconds(0.05f);
            }
        }      
        private IEnumerator TimerCoroutine(int durationInSeconds, System.Action onComplete)
        {
            int remainingTime = durationInSeconds;

            while (remainingTime > 0)
            {
                // Форматируем время в формате MM:SS
                string formattedTime = string.Format("{0:D2}:{1:D2}", remainingTime / 60, remainingTime % 60);
                timerText.text = formattedTime;

                // Уменьшаем оставшееся время и ждем 1 секунду
                remainingTime--;
                yield return new WaitForSeconds(1f);
            }

            // Когда таймер заканчивается, обновляем UI и вызываем метод onComplete
            timerText.text = "00:00";
            onComplete?.Invoke();
        }
    }
}
