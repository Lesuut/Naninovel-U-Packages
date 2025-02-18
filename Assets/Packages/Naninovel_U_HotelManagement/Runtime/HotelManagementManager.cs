using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.HotelManagement
{
    [InitializeAtRuntime()]
    public class HotelManagementManager : IHotelManagementManager
    {
        public virtual HotelManagementConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private IUIManager uIManager;
        private HotelManagementState state;

        private HotelManagementUI hotelManagementUI;
        private List<HotelRoomController> hotelRoomControllers;

        private CoroutinePlayer coroutinePlayer;

        private int guestsInLineCount;

        private Coroutine coroutineSpawnGuests;

        private MiniGameEventsType miniGameEventsType;

        public HotelManagementManager(HotelManagementConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
            hotelRoomControllers = new List<HotelRoomController>();
            guestsInLineCount = 0;
        }
        public UniTask InitializeServiceAsync()
        {
            state = new HotelManagementState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            coroutinePlayer = Engine.CreateObject<CoroutinePlayer>();

            uIManager = Engine.GetService<IUIManager>();

            miniGameEventsType = MiniGameEventsType.Null;

            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() { }

        private void Serialize(GameStateMap map) => map.SetState(new HotelManagementState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<HotelManagementState>();
            state = state == null ? new HotelManagementState() : new HotelManagementState(state);

            return UniTask.CompletedTask;
        }

        public void StartMiniGame(string levelName)
        {
            TryUIInit();
            hotelManagementUI.Show();

            HotelLevelInfo hotelLevelInfo = Configuration.GetLevel(levelName);

            foreach (var item in hotelRoomControllers)
                item.SetLevel(hotelLevelInfo);

            coroutineSpawnGuests = coroutinePlayer.StartCoroutine(CurSpawnGuests(hotelLevelInfo));
        }

        public void Improve(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool IsHotelWin()
        {
            throw new System.NotImplementedException();
        }

        private void FinishGame()
        {

        }

        private void ReceptionButton()
        {
            Debug.Log("Reception Button");
            if (!hotelManagementUI.hotelMovementSystem.IsMove && guestsInLineCount > 0)
            {
                hotelManagementUI.hotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.player, hotelManagementUI.ButtonReception.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        miniGameEventsType = MiniGameEventsType.Reception;
                    });
            }
        }

        private void FoodButton()
        {
            Debug.Log("Food Button");

            if (!hotelManagementUI.hotelMovementSystem.IsMove && hotelRoomControllers.Any(item => item.miniGameEventsType == MiniGameEventsType.Food))
            {
                hotelManagementUI.hotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.player, hotelManagementUI.ButtonFood.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        miniGameEventsType = MiniGameEventsType.Food;
                    });
            }
        }

        private void CleaningButton()
        {
            Debug.Log("Cleaning Button");

            if (!hotelManagementUI.hotelMovementSystem.IsMove && hotelRoomControllers.Any(item => item.miniGameEventsType == MiniGameEventsType.Cleaning))
            {
                hotelManagementUI.hotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.player, hotelManagementUI.ButtonCleaning.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        miniGameEventsType = MiniGameEventsType.Cleaning;
                    });
            }
        }

        private IEnumerator CurSpawnGuests(HotelLevelInfo hotelLevelInfo)
        {
            while (true)
            {
                guestsInLineCount++;
                Debug.Log($"guestsInLineCount: {guestsInLineCount}");
                hotelManagementUI.ShowKey();
                yield return new WaitForSeconds(Random.Range(hotelLevelInfo.MinGuestsSpawnTime, hotelLevelInfo.MaxGuestsSpawnTime)); 
            }
        }

        private void TryDoorAction(HotelRoomController hotelRoomController)
        {
            switch (miniGameEventsType)
            {
                case MiniGameEventsType.Food:
                    if (!hotelRoomController.Empty &&
                        !hotelManagementUI.hotelMovementSystem.IsMove)
                    {
                        hotelManagementUI.hotelMovementSystem.MoveRectToHotelTargetCur(
                                hotelManagementUI.player,
                                hotelRoomController.doorUIItem.rectTransform.anchoredPosition,
                                () =>
                                {
                                    hotelRoomController.CompleteMiniGame();
                                    miniGameEventsType = MiniGameEventsType.Null;
                                });
                    }
                    break;
                case MiniGameEventsType.Cleaning:
                    if (!hotelRoomController.Empty &&
                        !hotelManagementUI.hotelMovementSystem.IsMove)
                    {
                        hotelManagementUI.hotelMovementSystem.MoveRectToHotelTargetCur(
                                hotelManagementUI.player,
                                hotelRoomController.doorUIItem.rectTransform.anchoredPosition,
                                () =>
                                {
                                    hotelRoomController.CompleteMiniGame();
                                    miniGameEventsType = MiniGameEventsType.Null;
                                });
                    }
                    break;
                case MiniGameEventsType.Reception:
                    if (guestsInLineCount > 0 &&
                        !hotelManagementUI.hotelMovementSystem.IsMove &&
                        miniGameEventsType == MiniGameEventsType.Reception &&
                        hotelRoomController.Empty)
                        {
                            guestsInLineCount--;
                            if (guestsInLineCount <= 0)
                            {
                                hotelManagementUI.HideKey();
                            }

                            hotelManagementUI.hotelMovementSystem.MoveRectToHotelTargetCur(
                                hotelManagementUI.player,
                                hotelRoomController.doorUIItem.rectTransform.anchoredPosition,
                                () =>
                                {
                                    hotelRoomController.Occupy();
                                    hotelRoomController.StartMood();
                                    miniGameEventsType = MiniGameEventsType.Null;
                                });
                    }
                    break;
                default:
                    break;
            }          
        }

        private void TryUIInit()
        {
            if (hotelManagementUI == null)
            {
                hotelManagementUI = uIManager.GetUI<HotelManagementUI>();
                hotelManagementUI.ButtonReception.onClick.AddListener(ReceptionButton);
                hotelManagementUI.ButtonFood.onClick.AddListener(FoodButton);
                hotelManagementUI.ButtonCleaning.onClick.AddListener(CleaningButton);

                foreach (var item in hotelManagementUI.doorUIItems)
                {
                    HotelRoomController hotelRoomController = new HotelRoomController(
                        item,
                        Configuration,
                        coroutinePlayer,
                        (float mood) => Debug.Log($"Finish Mood: {mood}")
                        );

                    hotelRoomController.Reset();

                    hotelRoomControllers.Add(hotelRoomController);

                    item.Init(() => TryDoorAction(hotelRoomController));
                }
            }
        }
    }
}
