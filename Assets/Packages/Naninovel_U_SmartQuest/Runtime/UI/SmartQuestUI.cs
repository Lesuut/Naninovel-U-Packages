using UnityEngine;
using Naninovel.UI;
using UnityEngine.Events;

namespace Naninovel.U.SmartQuest
{
    public class SmartQuestUI : CustomUI
    {
        [SerializeField] private UnityEvent<string> updateQuestTextAction;

        private ISmartQuestService SmartQuestService;

        protected override void Awake()
        {
            base.Awake();

            SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.UpdateQuestTextAction += text => updateQuestTextAction?.Invoke(text);
        }
    }
}
