namespace Naninovel.U.LibraryAudio
{
    [CommandAlias("stopUsfx")]
    public class StopUSfx : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter BgmKey;

        [ParameterDefaultValue("")]
        public StringParameter Group = "";

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var audioLibraryManager = Engine.GetService<AudioLibraryManager>();

            audioLibraryManager.StopSfx(BgmKey, Group);

            return UniTask.CompletedTask;
        }
    }
}
