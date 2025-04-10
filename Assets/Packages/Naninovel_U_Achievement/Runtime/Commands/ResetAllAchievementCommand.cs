namespace Naninovel.U.Achievement.Commands
{
    [CommandAlias("resetAllAch")]
    public class ResetAllAchievementCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            Engine.GetService<IAchievementService>().ResetAllAchievement();
            return UniTask.CompletedTask;
        }
    }
}