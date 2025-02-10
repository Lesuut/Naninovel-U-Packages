using UnityEngine;
using UnityEngine.UI;
using Naninovel.UI;

namespace Naninovel.U.SmartQuest
{
    public class SmartQuestUI : CustomUI
    {
        private ISmartQuestService SmartQuestService;

        protected override void Awake()
        {
            base.Awake();

            SmartQuestService = Engine.GetService<ISmartQuestService>();
        }

        /// <summary>
        /// Write the body for the SmartQuest UI here
        /// </summary>
    }
}