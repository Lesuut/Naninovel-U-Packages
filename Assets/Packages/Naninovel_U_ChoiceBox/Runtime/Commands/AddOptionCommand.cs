namespace Naninovel.U.ChoiceBox.Commands
{
    [CommandAlias("addChoiceBoxOption")]
    public class AddOptionCommand : Command, Command.ILocalizable
    {
        [ParameterAlias(NamelessParameterAlias), LocalizableParameter]
        public StringParameter firstValue;

        [ParameterAlias("todo")]
        public StringParameter text = "";

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ChoiceBoxService = Engine.GetService<IChoiceBoxService>();

            ChoiceBoxService.AddOption(firstValue, text);

            return UniTask.CompletedTask;
        }
    }
}