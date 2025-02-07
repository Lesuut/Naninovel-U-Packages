namespace Naninovel.U.ChoiceBox.Commands
{
    [CommandAlias("showChoiceBox")]
    public class ShowChoiceBoxCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ChoiceBoxService = Engine.GetService<IChoiceBoxService>();

            ChoiceBoxService.ShowChoiceBox();

            return UniTask.CompletedTask;
        }
    }
}