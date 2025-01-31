namespace Naninovel.U.LibraryAudio
{
    [CommandAlias("stopUbgm")]
    public class StopUBgm : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter BgmKey;

        [ParameterDefaultValue("")]
        public StringParameter Group = "";

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var audioLibraryManager = Engine.GetService<AudioLibraryManager>();

            audioLibraryManager.StopBmg(BgmKey);

            return UniTask.CompletedTask;
        }
    }
}
