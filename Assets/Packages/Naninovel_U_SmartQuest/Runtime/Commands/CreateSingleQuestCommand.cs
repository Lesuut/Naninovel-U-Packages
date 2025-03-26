namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("createSingleQuest")]
    public class CreateSingleQuestCommand : Command, Command.ILocalizable
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter id;

        [ParameterAlias("title"), LocalizableParameter, RequiredParameter]
        public StringParameter title;

        [ParameterAlias("description"), LocalizableParameter]
        public StringParameter description;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.CreateSingleQuest(id.Value, title.Value, description.Value);

            return UniTask.CompletedTask;
        }
    }
}