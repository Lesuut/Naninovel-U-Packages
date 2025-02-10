namespace Naninovel.U.SmartQuest.Commands
{
    [CommandAlias("createsinglequest")]
    public class CreateSingleQuestCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var SmartQuestService = Engine.GetService<ISmartQuestService>();

            return UniTask.CompletedTask;
        }
    }
}