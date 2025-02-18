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
            if (!hotelManagementUI.HotelMovementSystem.IsMove && guestsInLineCount > 0)
            {
                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.PlayerRectTransform, hotelManagementUI.ButtonReception.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        miniGameEventsType = MiniGameEventsType.Reception;
                        hotelManagementUI.SetPlayerItem(Configuration.GetIcone(miniGameEventsType));
                        hotelManagementUI.SetReceptionKeyView(guestsInLineCount - 1);
                    });
            }
        }

        private void FoodButton()
        {
            if (!hotelManagementUI.HotelMovementSystem.IsMove && hotelRoomControllers.Any(item => item.miniGameEventsType == MiniGameEventsType.Food))
            {
                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.PlayerRectTransform, hotelManagementUI.ButtonFood.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        miniGameEventsType = MiniGameEventsType.Food;
                        hotelManagementUI.SetPlayerItem(Configuration.GetIcone(miniGameEventsType));
                    });
            }
        }

        private void CleaningButton()
        {
            if (!hotelManagementUI.HotelMovementSystem.IsMove && hotelRoomControllers.Any(item => item.miniGameEventsType == MiniGameEventsType.Cleaning))
            {
                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.PlayerRectTransform, hotelManagementUI.ButtonCleaning.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        miniGameEventsType = MiniGameEventsType.Cleaning;
                        hotelManagementUI.SetPlayerItem(Configuration.GetIcone(miniGameEventsType));
                    });
            }
        }

        private IEnumerator CurSpawnGuests(HotelLevelInfo hotelLevelInfo)
        {
            while (true)
            {
                guestsInLineCount++;
                hotelManagementUI.SetReceptionKeyView(guestsInLineCount);
                yield return new WaitForSeconds(Random.Range(hotelLevelInfo.MinGuestsSpawnTime, hotelLevelInfo.MaxGuestsSpawnTime)); 
            }
        }

        private void TryDoorAction(HotelRoomController hotelRoomController)
        {
            switch (miniGameEventsType)
            {
                case MiniGameEventsType.Food:
                    if (!hotelRoomController.Empty &&
                        !hotelManagementUI.HotelMovementSystem.IsMove &&
                        hotelRoomController.miniGameEventsType == miniGameEventsType)
                    {
                        hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(
                                hotelManagementUI.PlayerRectTransform,
                                hotelRoomController.doorUIItem.rectTransform.anchoredPosition,
                                () =>
                                {
                                    hotelRoomController.CompleteMiniGame();
                                    miniGameEventsType = MiniGameEventsType.Null;
                                    hotelManagementUI.HidePlayerItem();
                                });
                    }
                    break;
                case MiniGameEventsType.Cleaning:
                    if (!hotelRoomController.Empty &&
                        !hotelManagementUI.HotelMovementSystem.IsMove &&
                        hotelRoomController.miniGameEventsType == miniGameEventsType)
                    {
                        hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(
                                hotelManagementUI.PlayerRectTransform,
                                hotelRoomController.doorUIItem.rectTransform.anchoredPosition,
                                () =>
                                {
                                    hotelRoomController.CompleteMiniGame();
                                    miniGameEventsType = MiniGameEventsType.Null;
                                    hotelManagementUI.HidePlayerItem();
                                });
                    }
                    break;
                case MiniGameEventsType.Reception:
                    if (guestsInLineCount > 0 &&
                        hotelRoomController.Empty &&
                        !hotelManagementUI.HotelMovementSystem.IsMove)
                        {
                            guestsInLineCount--;
                            hotelManagementUI.SetReceptionKeyView(guestsInLineCount);

                                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(
                                hotelManagementUI.PlayerRectTransform,
                                hotelRoomController.doorUIItem.rectTransform.anchoredPosition,
                                () =>
                                {
                                    hotelRoomController.Occupy();
                                    hotelRoomController.StartMood();
                                    miniGameEventsType = MiniGameEventsType.Null;
                                    hotelManagementUI.HidePlayerItem();
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

                foreach (var item in hotelManagementUI.DoorUIItems)
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
