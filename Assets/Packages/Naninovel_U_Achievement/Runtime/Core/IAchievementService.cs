namespace Naninovel.U.Achievement
{
    public interface IAchievementService : IEngineService
    {
        public void UnlockAchievement(string key);
        public bool IsAchievementGranted(string key);

        public void ResetAchievement(string key);
        public void ResetAllAchievement();
    }
}