using Naninovel;

namespace Steam
{
    [CommandAlias("resetAch")]
    public class ResetAchievementCommand: Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter AchName;
        
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var steamAchievement = Engine.GetService<ISteamAchievement>();
            steamAchievement.ClearAchievement(AchName);
            return UniTask.CompletedTask;
        }
    }
}