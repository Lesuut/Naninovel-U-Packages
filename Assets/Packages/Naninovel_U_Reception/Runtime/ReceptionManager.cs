using Naninovel.Commands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naninovel.U.Reception
{
    [InitializeAtRuntime()]
    public class ReceptionManager : IReceptionManager
    {
        public virtual ReceptionConfiguration Configuration { get; }

        private ReceptionState state;
        private readonly IStateManager stateManager;

        private IUIManager uIManager;
        private ITextManager textManager;
        private IScriptPlayer scriptPlayer;
        private ITextPrinterManager textPrinter;

        private ReceptionUI receptionUI;

        public ReceptionManager(ReceptionConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;
        }

        public UniTask InitializeServiceAsync()
        {
            state = new ReceptionState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            uIManager = Engine.GetService<IUIManager>();

            textManager = Engine.GetService<ITextManager>();
            scriptPlayer = Engine.GetService<IScriptPlayer>();
            textPrinter = Engine.GetService<ITextPrinterManager>();

            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() { }

        private void Serialize(GameStateMap map) => map.SetState(new ReceptionState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<ReceptionState>();
            state = state == null ? new ReceptionState() : new ReceptionState(state);

            if (state.pairIdsChain.Count > 0)
            {
                if (receptionUI == null) receptionUI = uIManager.GetUI<ReceptionUI>();
                receptionUI.Show();
                ChainPairSwitcher();
            }

            return UniTask.CompletedTask;
        }

        public void PlayReceptionMiniGame(int cardCount)
        {
            if (cardCount > Configuration.Pairs.Length && cardCount <= 0) return;
            if (receptionUI == null) receptionUI = uIManager.GetUI<ReceptionUI>();
            receptionUI.Show();

            state.correctAnswer = 0;
            state.incorrectAnswer = 0;

            state.scriptPlayedIndex = scriptPlayer.PlayedIndex;
            scriptPlayer.Stop();

            state.pairIdsChain = GetRandomPairIds(cardCount).ToList();

            ChainPairSwitcher();
        }

        private bool pairActive = false;

        private void ChainPairSwitcher()
        {
            if (state.pairIdsChain.Count <= 0)
            {
                scriptPlayer.Play(scriptPlayer.Playlist, state.scriptPlayedIndex + 1);
                state.pairIdsChain.Clear();
                receptionUI.Hide();
                return;
            }

            pairActive = true;

            Debug.Log($"{string.Join(", ", state.pairIdsChain)}");

            int id = state.pairIdsChain[0];

            receptionUI.Printer(
                textManager.GetRecordValue($"Reception.Human.Name.{id}", Configuration.textCategory),
                textManager.GetRecordValue($"Reception.Human.RoomDescription.{id}", Configuration.textCategory));

            receptionUI.ShowPair(
                GetScreenText(id), 
                GetCardText(id), 
                () => AcceptAction(id), 
                () => CancelAction(id), 
                ChainPairSwitcher);
        }

        private void CancelAction(int id)
        {
            if (pairActive)
            {
                if (!Configuration.Pairs[id])
                {
                    Debug.Log("+");
                    state.correctAnswer++;
                }
                else
                {
                    Debug.Log("-");
                    state.incorrectAnswer++;
                }

                receptionUI.CancelAnim();

                pairActive = false;
                state.pairIdsChain.RemoveAt(0);
            }
        }
        private void AcceptAction(int id)
        {
            if (pairActive)
            {
                if (Configuration.Pairs[id])
                {
                    Debug.Log("+");
                    state.correctAnswer++;
                }
                else
                {
                    Debug.Log("-");
                    state.incorrectAnswer++;
                }

                receptionUI.AcceptAnim();

                pairActive = false;
                state.pairIdsChain.RemoveAt(0);
            }
        }

        private string GetScreenText(int id)
        {
            string name = textManager.GetRecordValue($"Reception.Screen.Name.{id}", Configuration.textCategory);
            string dateBirth = textManager.GetRecordValue($"Reception.Screen.DateOfBirth.{id}", Configuration.textCategory);

            return 
                $"{(string.IsNullOrEmpty(name) ? "" : $"{name}\n")}" +
                $"{(string.IsNullOrEmpty(dateBirth) ? "" : $"{dateBirth}\n")}" +
                $"{textManager.GetRecordValue($"Reception.Screen.RoomDescription.{id}", Configuration.textCategory)}\n" +
                $"{textManager.GetRecordValue($"Reception.Screen.Date.{id}", Configuration.textCategory)}";
        }

        private string GetCardText(int id)
        {
            return
                $"{textManager.GetRecordValue($"Reception.Human.Name.{id}", Configuration.textCategory)}\n" +
                $"{textManager.GetRecordValue($"Reception.Human.DateOfBirth.{id}", Configuration.textCategory)}\n" +
                //$"{textManager.GetRecordValue($"Reception.Human.RoomDescription.{id}", Configuration.textCategory)}\n" +
                $"{textManager.GetRecordValue($"Reception.Human.Date.{id}", Configuration.textCategory)}";
        }

        private int[] GetRandomPairIds(int cardCount)
        {
            HashSet<int> uniquePairIds = new HashSet<int>();

            while (uniquePairIds.Count < cardCount)
            {
                uniquePairIds.Add(Random.Range(0, Configuration.Pairs.Length));
            }

            return uniquePairIds.ToArray();
        }

        public bool IsReceptionWin() => state.correctAnswer > state.incorrectAnswer;
    }
}
