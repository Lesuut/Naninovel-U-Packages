namespace Naninovel.U.SmartQuest
{
    [ExpressionFunctions]
    public static class SmartQuestFunctions
    {
        public static string GetQuestStatus()
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            return "";
        }
    }
}