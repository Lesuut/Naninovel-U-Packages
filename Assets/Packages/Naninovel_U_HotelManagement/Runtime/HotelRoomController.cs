using System;
using System.Collections;
using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    public class HotelRoomController
    {
        public bool Empty { get; private set; }
        public MiniGameEventsType miniGameEventsType { get; private set; }
        public DoorUIItem doorUIItem { get; private set; }

        private MonoBehaviour monoBehaviour;
        private HotelManagementConfiguration configuration;
        private HotelLevelInfo hotelLevelInfo;
        private Action<float> finishMoodAction;

        private float mood;
        private Coroutine coroutine;
        private Coroutine lessModCoroutine;

        public HotelRoomController(DoorUIItem doorUIItem, HotelManagementConfiguration configuration, MonoBehaviour monoBehaviour, Action<float> finishMoodAction) 
        {
            Empty = true;
            miniGameEventsType = MiniGameEventsType.Null;
            this.doorUIItem = doorUIItem;
            this.monoBehaviour = monoBehaviour;
            this.configuration = configuration;
            this.finishMoodAction = finishMoodAction;
            mood = 1;
        }

        public void SetLevel(HotelLevelInfo hotelLevelInfo) => this.hotelLevelInfo = hotelLevelInfo;

        public void Reset()
        {
            if (coroutine != null)
                monoBehaviour.StopCoroutine(coroutine);

            if (lessModCoroutine != null)
                monoBehaviour.StopCoroutine(lessModCoroutine);

            miniGameEventsType = MiniGameEventsType.Null;
            Empty = true;
            mood = 1;
            Release();
        }
        public void StartMood()
        {
            lessModCoroutine = monoBehaviour.StartCoroutine(curLessMood());
        }
        public void Occupy()
        {
            Empty = false;
            doorUIItem.OccupyDoor();
            coroutine = monoBehaviour.StartCoroutine(curLiveActiveLine());
        }
        public void Release()
        {
            doorUIItem.ReleaseDoor();
            doorUIItem.HideIcone();
            doorUIItem.SetMood(0);
        }
        public void CompleteMiniGame()
        {
            miniGameEventsType = MiniGameEventsType.Null;
        }
        private IEnumerator curLiveActiveLine()
        {
            for (int i = 0; i < UnityEngine.Random.Range(hotelLevelInfo.MinNumberFoodOrders, hotelLevelInfo.MaxNumberFoodOrders); i++)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(hotelLevelInfo.MinTimeFoodOrders, hotelLevelInfo.MaxTimeFoodOrders));
                miniGameEventsType = MiniGameEventsType.Food;
                doorUIItem.SetIcone(configuration.GetIcone(MiniGameEventsType.Food));
                yield return new WaitUntil(() => miniGameEventsType == MiniGameEventsType.Null);
                mood += hotelLevelInfo.MoodPlusValue;
                doorUIItem.HideIcone();
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(hotelLevelInfo.MinCliningTime, hotelLevelInfo.MaxCliningTime));
            miniGameEventsType = MiniGameEventsType.Cleaning;
            doorUIItem.SetIcone(configuration.GetIcone(MiniGameEventsType.Cleaning));
            yield return new WaitUntil(() => miniGameEventsType == MiniGameEventsType.Null);
            mood += hotelLevelInfo.MoodPlusValue;

            finishMoodAction?.Invoke(Mathf.Clamp01(mood));

            if (lessModCoroutine != null)
                monoBehaviour.StopCoroutine(lessModCoroutine);

            doorUIItem.HideIcone();

            Release();
            Empty = true;
            mood = 1;
        }
        private IEnumerator curLessMood()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                mood = Mathf.Clamp01(mood - hotelLevelInfo.MoodLossSpeed / 2f);
                doorUIItem.SetMood(mood);
            }
        }
    } 
}
