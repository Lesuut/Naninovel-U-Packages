namespace Naninovel.U.CrossPromo.Commands
{
    [CommandAlias("addCrossPromo")]
    public class AddCrossPromoItemCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public IntegerParameter id;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var CrossPromoService = Engine.GetService<ICrossPromoService>();

            CrossPromoService.UnlockItem(id);

            return UniTask.CompletedTask;
        }
    }
}
