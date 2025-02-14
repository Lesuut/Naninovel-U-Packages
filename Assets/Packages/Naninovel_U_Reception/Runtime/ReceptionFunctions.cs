namespace Naninovel.U.Reception
{
    [ExpressionFunctions]
    public class ReceptionFunctions
    {
        public static bool IsReceptionWin()
        {
            var SmartQuestService = Engine.GetService<IReceptionManager>();

            return SmartQuestService.IsReceptionWin();
        }
    }
}
