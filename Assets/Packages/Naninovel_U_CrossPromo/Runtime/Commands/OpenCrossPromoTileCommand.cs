namespace Naninovel.U.CrossPromo.Commands
{
    [CommandAlias("opencrosspromotile")]
    public class OpenCrossPromoTileCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var CrossPromoService = Engine.GetService<ICrossPromoService>();

            return UniTask.CompletedTask;
        }
    }
}