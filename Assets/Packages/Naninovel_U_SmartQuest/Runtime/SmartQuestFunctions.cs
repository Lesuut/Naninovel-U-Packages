namespace Naninovel.U.SmartQuest
{
    [ExpressionFunctions]
    public static class SmartQuestFunctions
    {
        public static bool GetQuestStatus(string idQuest)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            return SmartQuestService.GetQuestStatus(idQuest);
        }
    }
}