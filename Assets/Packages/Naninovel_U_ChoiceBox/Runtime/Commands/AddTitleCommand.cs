namespace Naninovel.U.ChoiceBox.Commands
{
    [CommandAlias("addChoiceBoxTitle")]
    public class AddTitleCommand : Command, Command.ILocalizable
    {
        [ParameterAlias(NamelessParameterAlias), LocalizableParameter]
        public StringParameter firstValue;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ChoiceBoxService = Engine.GetService<IChoiceBoxService>();

            ChoiceBoxService.SetTitle(firstValue);

            return UniTask.CompletedTask;
        }
    }
}