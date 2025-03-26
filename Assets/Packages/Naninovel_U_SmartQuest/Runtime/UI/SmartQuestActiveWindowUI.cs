using UnityEngine;
using Naninovel.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Text.RegularExpressions;
using System.Linq;

namespace Naninovel.U.SmartQuest
{
    public class SmartQuestActiveWindowUI : CustomUI
    {
        [SerializeField] private UnityEvent<string> updateTitleTextAction;
        [Space]
        [SerializeField] private RectTransform windowRectTransform;
        [SerializeField] private Vector2 showPos;
        [SerializeField] private Vector2 hidePos;
        [SerializeField] private float duration = 1;

        private ISmartQuestService smartQuestService;

        protected override void Awake()
        {
            base.Awake();
            smartQuestService = Engine.GetService<ISmartQuestService>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            smartQuestService.UpdateQuestAction += UpdateQuest;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            smartQuestService.UpdateQuestAction -= UpdateQuest;
        }

        private void UpdateQuest()
        {
            var quests = smartQuestService.GetAllQuests();
            if (quests.Length == 0) return;

            // Берем первый активный квест (не завершенный)
            Quest activeQuest = quests.FirstOrDefault(q => !q.IsQuestComplete());

            if (activeQuest == null)
            {
                // Все квесты завершены, просто скрываем окно
                windowRectTransform.DOAnchorPos(hidePos, duration).onComplete += Hide;
                return;
            }

            // Проверяем, завершился ли текущий отображаемый квест
            Quest currentQuest = quests[0];

            if (currentQuest.IsQuestComplete())
            {
                // Если да, сначала скрываем окно, а потом меняем квест и снова показываем
                windowRectTransform.DOAnchorPos(hidePos, duration).onComplete += () =>
                {
                    ShowNewQuest(activeQuest);
                };
            }
            else
            {
                // Если текущий квест не завершен, просто обновляем окно
                ShowNewQuest(currentQuest);
            }
        }

        private void ShowNewQuest(Quest quest)
        {
            Show();
            windowRectTransform.DOAnchorPos(showPos, duration);
            updateTitleTextAction?.Invoke(RemoveColorTags(quest.GetQuestTitle()));
        }

        private string RemoveColorTags(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string pattern = @"<color=[^>]+>|</color>";
            return Regex.Replace(input, pattern, "");
        }
    }
}
