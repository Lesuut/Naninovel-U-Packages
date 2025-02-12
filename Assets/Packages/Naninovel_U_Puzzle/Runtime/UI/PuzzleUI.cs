using UnityEngine;
using Naninovel.UI;
using System;
using System.Linq;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

namespace Naninovel.U.Puzzle
{
    public class PuzzleUI : CustomUI
    {
        [Serializable]
        private new class GameState
        {
            public string puzzleName;
            public Vector2[] finalPositions;
            public Vector2[] currentPositions;

            public string startScriptName;
            public int startScriptPlayedIndex;
        }

        private GameState gameState;

        [SerializeField] private Canvas canvas;
        [SerializeField] private float magnetRadius = 50;
        [Space]
        [SerializeField] private float waitBeforeFinish = 1;
        [SerializeField] private UnityEvent finish;

        private IScriptPlayer scriptPlayer;

        private PuzzlePartsKit[] puzzlePartsKits;
        private RectTransform currentSelectPiceRectTransform;
        private Vector2 offset; // Смещение между позицией мыши и объектом
        private PuzzlePartsKit currentPuzzlePartsKit;

        private bool mixAnim = false;

        protected override void OnEnable()
        {
            puzzlePartsKits = GetComponentsInChildren<PuzzlePartsKit>();
            foreach (var item in puzzlePartsKits)
                item.Hide();

            scriptPlayer = Engine.GetService<IScriptPlayer>();

            base.OnEnable();
        }

        public void StartPuzzleMiniGame(string puzzleName)
        {
            if (puzzleName == "") return;

            if (currentPuzzlePartsKit == null)
                currentPuzzlePartsKit = puzzlePartsKits.FirstOrDefault(item => item.Name == puzzleName);

            gameState.puzzleName = puzzleName;
            gameState.finalPositions = currentPuzzlePartsKit.GetPartsPositions();

            gameState.startScriptName = scriptPlayer.Playlist.ScriptName;
            gameState.startScriptPlayedIndex = scriptPlayer.PlayedIndex;
            scriptPlayer.Stop();

            Show();
            currentPuzzlePartsKit.Show();

            mixAnim = true;
            ShufflePuzzlePiecesAnim();
        }

        private void ShufflePuzzlePiecesAnim()
        {
            currentPuzzlePartsKit.GetPartsCanvasGroup().alpha = 0;

            Color currentColor = currentPuzzlePartsKit.GetCollectedImage().color;
            currentPuzzlePartsKit.GetCollectedImage().color = new Color(currentColor.r, currentColor.g, currentColor.b, 1);

            Sequence mySequence = DOTween.Sequence();

            currentPuzzlePartsKit.GetCollectedImage().rectTransform.localScale = new Vector2(1.5f, 1.5f);
            mySequence.Append(currentPuzzlePartsKit.GetCollectedImage().rectTransform.DOScale(Vector2.one, 0.5f));

            mySequence.Append(currentPuzzlePartsKit.GetPartsCanvasGroup().DOFade(1, 0.1f));

            mySequence.Append(currentPuzzlePartsKit.GetCollectedImage().DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 0), 0.25f));

            foreach (var item in currentPuzzlePartsKit.GetAllParts())
            {
                Vector2 randomPos = new Vector2(
                    UnityEngine.Random.Range(RectTransform.rect.xMin, RectTransform.rect.xMax),
                    UnityEngine.Random.Range(RectTransform.rect.yMin, RectTransform.rect.yMax)
                );

                Vector2 clampedPos = ClampPositionToScreen(randomPos, item);

                mySequence.Append(item.DOScale(new Vector2(1.2f, 1.2f), 0.05f).SetEase(Ease.InOutQuad));

                mySequence.Append(item.DOAnchorPos(clampedPos, 0.1f));

                mySequence.Append(item.DOScale(Vector2.one, 0.05f).SetEase(Ease.InOutQuad));
            }

            mySequence.AppendCallback(() => mixAnim = false);

            mySequence.Play();
        }

        private void Update()
        {
            if (currentSelectPiceRectTransform == null || canvas == null || gameState == null || mixAnim) return;

            // Получаем целевую позицию
            Vector2 finalPosition = gameState.finalPositions[currentPuzzlePartsKit.GetPartIndex(currentSelectPiceRectTransform)];

            // Получаем текущую позицию мыши
            Vector2 mousePosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            // Учитываем смещение
            Vector2 currentPosition = localPoint - offset;

            // Проверяем расстояние от текущей позиции до целевой
            float distanceToFinalPosition = Vector2.Distance(currentPosition, finalPosition);

            // Если объект достаточно близко к целевой позиции, притягиваем его
            if (distanceToFinalPosition < magnetRadius)
            {
                currentSelectPiceRectTransform.anchoredPosition = finalPosition;
            }
            else
            {
                // Иначе продолжаем следовать за мышью, но ограничиваем позицию в пределах экрана
                currentSelectPiceRectTransform.anchoredPosition = ClampPositionToScreen(currentPosition, currentSelectPiceRectTransform);
            }
        }

        private Vector2 ClampPositionToScreen(Vector2 position, RectTransform rectTransform)
        {
            // Получаем размеры канваса
            Rect canvasRect = canvas.GetComponent<RectTransform>().rect;

            // Получаем размеры объекта
            Vector2 objectSize = rectTransform.rect.size;

            // Ограничиваем позицию объекта в пределах экрана
            float clampedX = Mathf.Clamp(position.x, canvasRect.xMin + objectSize.x / 2, canvasRect.xMax - objectSize.x / 2);
            float clampedY = Mathf.Clamp(position.y, canvasRect.yMin + objectSize.y / 2, canvasRect.yMax - objectSize.y / 2);

            return new Vector2(clampedX, clampedY);
        }

        public void SelectPice(RectTransform rectTransform)
        {
            currentSelectPiceRectTransform = rectTransform;

            // Сохраняем смещение в момент захвата
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            offset = localPoint - currentSelectPiceRectTransform.anchoredPosition;

            currentSelectPiceRectTransform.SetAsLastSibling();

            rectTransform.DOScale(new Vector2(1.2f, 1.2f), 0.1f).SetEase(Ease.InOutQuad);
        }

        public void UnselectPice()
        {
            currentSelectPiceRectTransform.DOScale(Vector2.one, 0.2f).SetEase(Ease.InOutQuad);

            currentSelectPiceRectTransform = null;

            StartCoroutine(TryFinish());
        }

        protected override void SerializeState(GameStateMap stateMap)
        {
            base.SerializeState(stateMap);

            if (gameState == null)
                gameState = new GameState();

            if (currentPuzzlePartsKit != null)
            {
                gameState.currentPositions = currentPuzzlePartsKit.GetPartsPositions();
            }

            stateMap.SetState(gameState);
        }

        protected override async UniTask DeserializeState(GameStateMap stateMap)
        {
            await base.DeserializeState(stateMap);

            gameState = stateMap.GetState<GameState>();
            if (gameState is null) return;

            if (gameState.puzzleName != "")
            {
                currentPuzzlePartsKit = puzzlePartsKits.FirstOrDefault(item => item.Name == gameState.puzzleName);
                currentPuzzlePartsKit.SetPartsPositions(gameState.currentPositions);

                currentPuzzlePartsKit.Show();
                Show();
            }
        }
        private IEnumerator TryFinish()
        {
            foreach (var currentPosItem in currentPuzzlePartsKit.GetPartsPositions())
            {
                bool find = false;
                foreach (var item in gameState.finalPositions)
                {
                    if (currentPosItem == item)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find) yield break;
            }

            finish?.Invoke();

            yield return new WaitForSeconds(waitBeforeFinish);

            yield return scriptPlayer.PreloadAndPlayAsync(gameState.startScriptName);
            scriptPlayer.Play(scriptPlayer.Playlist, gameState.startScriptPlayedIndex + 1);

            gameState = new GameState();
            Hide();
        }
    }
}
