namespace Naninovel.U.Achievement
{
    [ExpressionFunctions]
    public static class AchievementFunctions
    {
        public static bool IsAchievementGranted(string key)
        {
            return Engine.GetService<IAchievementService>().IsAchievementGranted(key);
        }
    }
}