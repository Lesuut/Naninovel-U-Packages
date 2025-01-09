namespace Naninovel.U.LibraryAudio
{
    [CommandAlias("usfx")]
    public class PlayUSfx : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter BgmKey;

        [ParameterDefaultValue("")]
        public StringParameter Group = "";

        [ParameterDefaultValue("false")]
        public BooleanParameter Loop = false;

        public DecimalParameter Volume = AudioLibraryConfiguration.defSfxVolume;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var audioLibraryManager = Engine.GetService<AudioLibraryManager>();

            audioLibraryManager.PlaySfx(BgmKey, Group, Loop, Volume);

            return UniTask.CompletedTask;
        }
    }
}