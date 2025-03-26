namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("executeMultiQuestOption")]
    public class ExecuteMultiQuestOptionCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter idQuest;

        [ParameterAlias("option"), RequiredParameter]
        public StringParameter idOption;

        [ParameterAlias("value")]
        public IntegerParameter value;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.ExecuteMultiQuestOption(idQuest.Value, idOption.Value, value.Value);

            return UniTask.CompletedTask;
        }
    }
}