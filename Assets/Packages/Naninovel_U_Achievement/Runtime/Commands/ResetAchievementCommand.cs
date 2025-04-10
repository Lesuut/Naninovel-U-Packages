namespace Naninovel.U.Achievement.Commands
{
    [CommandAlias("resetachievement")]
    public class ResetAchievementCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var AchievementService = Engine.GetService<IAchievementService>();

            return UniTask.CompletedTask;
        }
    }
}