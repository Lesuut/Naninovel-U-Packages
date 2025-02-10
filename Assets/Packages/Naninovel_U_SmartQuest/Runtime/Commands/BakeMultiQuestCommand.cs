namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("bakeMultiQuest")]
    public class BakeMultiQuestCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.UpdateInfoAction();

            return UniTask.CompletedTask;
        }
    }
}