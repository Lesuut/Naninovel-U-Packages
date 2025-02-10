namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("addMultiQuestOption")]
    public class AddMultiQuestOptionCommand : Command, Command.ILocalizable
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter idQuest;

        [ParameterAlias("option"), RequiredParameter]
        public StringParameter idOption;

        [ParameterAlias("description"), LocalizableParameter, RequiredParameter]
        public StringParameter description;

        [ParameterAlias("maxProgressUnits")]
        public IntegerParameter maxProgresUnits;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.AddMultiQuestOption(idQuest.Value, idOption.Value, maxProgresUnits.Value, description.Value);

            return UniTask.CompletedTask;
        }
    }
}