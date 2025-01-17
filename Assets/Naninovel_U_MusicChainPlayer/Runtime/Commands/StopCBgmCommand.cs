namespace Naninovel.U.MusicChainPlayer.Commands
{
    [CommandAlias("stopcbgm")]
    public class StopCBgmCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var MusicChainPlayerManager = Engine.GetService<IMusicChainPlayerManager>();

            MusicChainPlayerManager.StopCBgm();

            return UniTask.CompletedTask;
        }
    }
}