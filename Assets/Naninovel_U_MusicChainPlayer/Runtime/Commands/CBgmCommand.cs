namespace Naninovel.U.MusicChainPlayer.Commands
{
    [CommandAlias("cbgm")]
    public class CBgmCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var MusicChainPlayerManager = Engine.GetService<IMusicChainPlayerManager>();

            MusicChainPlayerManager.PlayCBgm(firstValue.Value.Split(","));

            return UniTask.CompletedTask;
        }
    }
}