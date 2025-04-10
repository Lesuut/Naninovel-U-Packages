using Naninovel;

namespace Steam
{
    [CommandAlias("ach")]
    public class AchievementCommand: Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter AchName;
        
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var steamAchievement = Engine.GetService<ISteamAchievement>();
            steamAchievement.SetAchievement(AchName);
            return UniTask.CompletedTask;
        }
    }
}