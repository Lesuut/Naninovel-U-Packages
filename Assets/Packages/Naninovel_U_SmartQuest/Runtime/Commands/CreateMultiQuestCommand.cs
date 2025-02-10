namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("createMultiQuest")]
    public class CreateMultiQuestCommand : Command, Command.ILocalizable
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter id;

        [ParameterAlias("title"), LocalizableParameter, RequiredParameter]
        public StringParameter title;

        [ParameterAlias("description"), LocalizableParameter, RequiredParameter]
        public StringParameter description;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.CreateMultiQuest(id.Value, title.Value, description.Value);

            return UniTask.CompletedTask;
        }
    }
}