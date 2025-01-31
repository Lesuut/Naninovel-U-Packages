namespace Naninovel.U.LibraryAudio
{
    [CommandAlias("ubgm")]
    public class PlayUBgm : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter BgmKey;

        [ParameterDefaultValue("")]
        public StringParameter Group = "";

        /*[ParameterDefaultValue("false")]
        public BooleanParameter Loop = false;

        [ParameterDefaultValue("1")]
        public DecimalParameter Volume = 1f;*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var audioLibraryManager = Engine.GetService<AudioLibraryManager>();

            audioLibraryManager.PlayBmg(BgmKey, Group);

            return UniTask.CompletedTask;
        }
    }
}