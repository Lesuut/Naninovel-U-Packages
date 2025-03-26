using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [ExpressionFunctions]
    public static class SmartQuestFunctions
    {
        public static bool GetQuestStatus(string idQuest)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            Debug.Log($"{idQuest} GetQuestStatus: {SmartQuestService.GetQuestStatus(idQuest)}");
            return SmartQuestService.GetQuestStatus(idQuest);
        }
    }
}