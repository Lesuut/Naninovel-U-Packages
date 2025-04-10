using Naninovel;

namespace Steam
{
    public interface ISteamAchievement: IEngineService
    {
        public void SetAchievement(string achName);
        public void ClearAchievement(string achName);
    }
}