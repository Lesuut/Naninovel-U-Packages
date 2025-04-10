namespace Naninovel.U.Achievement.Commands
{
    [CommandAlias("ach")]
    public class UnlockAchievementCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter key;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            Engine.GetService<IAchievementService>().UnlockAchievement(key);
            return UniTask.CompletedTask;
        }
    }
}