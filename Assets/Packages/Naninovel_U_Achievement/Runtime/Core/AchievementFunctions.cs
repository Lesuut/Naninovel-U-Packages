namespace Naninovel.U.Achievement
{
    [ExpressionFunctions]
    public static class AchievementFunctions
    {
        public static string IsAchievementGranted()
        {
            var AchievementService = Engine.GetService<IAchievementService>();

            return "";
        }
    }
}