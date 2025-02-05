namespace Naninovel.U.ParametersChoice.Commands
{
    [CommandAlias("showchoice")]
    public class ShowChoiceCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ParametersChoiceManager = Engine.GetService<IParametersChoiceManager>();

            return UniTask.CompletedTask;
        }
    }
}