namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("completeSingleQuest")]
    public class CompleteSingleQuestCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter id;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            SmartQuestService.CompleteSingleQuest(id.Value);

            return UniTask.CompletedTask;
        }
    }
}