namespace Naninovel.U.Achievement.Commands
{
    [CommandAlias("resetAch")]
    public class ResetAchievementCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter key;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            Engine.GetService<IAchievementService>().ResetAchievement(key);
            return UniTask.CompletedTask;
        }
    }
}