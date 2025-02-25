using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Naninovel.U.HotelManagement
{
    [InitializeAtRuntime()]
    public class HotelManagementManager : IHotelManagementManager
    {
        public virtual HotelManagementConfiguration Configuration { get; }

        private readonly IStateManager stateManager;
        private IUIManager uIManager;
        private IScriptPlayer scriptPlayer;
        private HotelManagementState state;

        private HotelManagementUI hotelManagementUI;
        private List<HotelRoomController> hotelRoomControllers;

        private CoroutinePlayer coroutinePlayer;

        private int guestsInLineCount;
        private bool nowActionProgress = false;

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
            scriptPlayer = Engine.GetService<IScriptPlayer>();

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

            foreach (var item in hotelRoomControllers)
                item.Reset();
            hotelManagementUI.HidePlayerItem();

            if (state.GameActive)
                StartMiniGame(state.LevelKey);

            return UniTask.CompletedTask;
        }

        public void StartMiniGame(string levelName)
        {
            nowActionProgress = false;
            state.GameActive = true;
            state.LevelKey = levelName;
            coroutinePlayer.StopAllCoroutines();

            state.ScriptName = scriptPlayer.Playlist.ScriptName;
            state.ScriptPlayedIndex = scriptPlayer.PlayedIndex;
            scriptPlayer.Stop();

            HotelLevelInfo hotelLevelInfo = Configuration.GetLevel(levelName);

            if (!TryUIInit(hotelLevelInfo))
                hotelManagementUI.ResetUI();

            hotelManagementUI.Show();

            hotelManagementUI.SetReceptionUpgrade(state.ReceptionImproving);
            hotelManagementUI.SetFoodUpgrade(state.FoodImproving);
            hotelManagementUI.SetCleaningUpgrade(state.CleanImproving);

            hotelManagementUI.HotelMovementSystem.moveSpeed = hotelLevelInfo.MoveSpeed;

            hotelManagementUI.StartTimer(hotelLevelInfo.MiniGameTimeSeconds, FinishGame);

            foreach (var item in hotelRoomControllers)
                item.SetLevel(hotelLevelInfo);

            coroutinePlayer.StartCoroutine(CurSpawnGuests(hotelLevelInfo));

            state.CompletedMoods.Clear();
        }

        public void Improve(string key)
        {
            Debug.Log($"Improve hotel: {key}");
            switch (key)
            {
                case "Reception":
                    state.ReceptionImproving = 1;
                    break;
                case "Clean":
                    state.CleanImproving = 1;
                    break;
                case "Food":
                    state.FoodImproving = 1;
                    break;
                default:
                    break;
            }
        }

        public bool IsHotelWin()
        {
            if (state.CompletedMoods.Count > 1)
                return state.CompletedMoods.Average() > 0.5f;
            else
                return false;
        }

        private async void FinishGame()
        {
            state.GameActive = false;
            coroutinePlayer.StopAllCoroutines();

            foreach (var item in hotelRoomControllers)
                item.Reset();
            hotelManagementUI.HidePlayerItem();

            await scriptPlayer.PreloadAndPlayAsync(state.ScriptName);
            scriptPlayer.Play(scriptPlayer.Playlist, state.ScriptPlayedIndex + 1);

            hotelManagementUI.Hide();
        }

        private void ReceptionButton()
        {
            if (!hotelManagementUI.HotelMovementSystem.IsMove && guestsInLineCount > 0 && !nowActionProgress)
            {
                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.PlayerRectTransform, hotelManagementUI.ButtonReception.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        coroutinePlayer.StartCoroutine(ExecutionCoroutine(state.ReceptionImproving == 0 ? Configuration.actionTimeDuration : 0, 
                            hotelManagementUI.ReceptionProgressBar,
                            () => {
                                miniGameEventsType = MiniGameEventsType.Reception;
                                hotelManagementUI.SetPlayerItem(Configuration.GetIcone(miniGameEventsType));
                                hotelManagementUI.SetReceptionKeyView(guestsInLineCount - 1);
                            }));
                    });
            }
        }

        private void FoodButton()
        {
            if (!hotelManagementUI.HotelMovementSystem.IsMove && hotelRoomControllers.Any(item => item.miniGameEventsType == MiniGameEventsType.Food) && !nowActionProgress)
            {
                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.PlayerRectTransform, hotelManagementUI.ButtonFood.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        coroutinePlayer.StartCoroutine(ExecutionCoroutine(state.FoodImproving == 0 ? Configuration.actionTimeDuration : 0,
                            hotelManagementUI.FoodProgressBar,
                            () => {
                                miniGameEventsType = MiniGameEventsType.Food;
                                hotelManagementUI.SetPlayerItem(Configuration.GetIcone(miniGameEventsType));
                            }));
                    });
            }
        }

        private void CleaningButton()
        {
            if (!hotelManagementUI.HotelMovementSystem.IsMove && hotelRoomControllers.Any(item => item.miniGameEventsType == MiniGameEventsType.Cleaning) && !nowActionProgress)
            {
                hotelManagementUI.HotelMovementSystem.MoveRectToHotelTargetCur(hotelManagementUI.PlayerRectTransform, hotelManagementUI.ButtonCleaning.GetComponent<RectTransform>().anchoredPosition,
                    () =>
                    {
                        coroutinePlayer.StartCoroutine(ExecutionCoroutine(state.CleanImproving == 0 ? Configuration.actionTimeDuration : 0,
                            hotelManagementUI.CleaningProgressBar,
                            () => {
                                miniGameEventsType = MiniGameEventsType.Cleaning;
                                hotelManagementUI.SetPlayerItem(Configuration.GetIcone(miniGameEventsType));
                            }));
                    });
            }
        }

        private IEnumerator CurSpawnGuests(HotelLevelInfo hotelLevelInfo)
        {
            while (true)
            {
                guestsInLineCount++;
                hotelManagementUI.SetReceptionKeyView(guestsInLineCount);
                yield return new WaitForSeconds(UnityEngine.Random.Range(hotelLevelInfo.MinGuestsSpawnTime, hotelLevelInfo.MaxGuestsSpawnTime)); 
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

        private bool TryUIInit(HotelLevelInfo hotelLevelInfo)
        {
            if (hotelManagementUI == null)
            {
                hotelManagementUI = uIManager.GetUI<HotelManagementUI>();
                hotelManagementUI.ButtonReception.onClick.AddListener(ReceptionButton);
                hotelManagementUI.ButtonFood.onClick.AddListener(FoodButton);
                hotelManagementUI.ButtonCleaning.onClick.AddListener(CleaningButton);

                hotelManagementUI.ResetUI();

                foreach (var item in hotelManagementUI.DoorUIItems)
                {
                    HotelRoomController hotelRoomController = new HotelRoomController(
                        item,
                        Configuration,
                        coroutinePlayer,
                        (float mood) => {
                            Debug.Log($"Finish Mood: {mood}");
                            hotelManagementUI.UpdateMoney((int)(hotelLevelInfo.MaxMoneyRevard * mood), item.rectTransform);
                            state.CompletedMoods.Add(mood);
                        });

                    hotelRoomController.Reset();

                    hotelRoomControllers.Add(hotelRoomController);

                    item.Init(() => TryDoorAction(hotelRoomController));
                }

                return true;
            }

            return false;
        }

        private IEnumerator ExecutionCoroutine(float duration, UnityEvent<float> onProgressUpdate, Action onComplete)
        {
            nowActionProgress = true;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / duration);
                onProgressUpdate.Invoke(progress);
                yield return null;
            }

            onProgressUpdate.Invoke(0f); // В конце прогресса
            onComplete.Invoke(); // Ивент завершения
            nowActionProgress = false;
        }
    }
}
