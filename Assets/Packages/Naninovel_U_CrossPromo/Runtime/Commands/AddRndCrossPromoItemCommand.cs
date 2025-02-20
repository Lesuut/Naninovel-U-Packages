namespace Naninovel.U.CrossPromo.Commands
{
    [CommandAlias("addRndCrossPromo")]
    public class AddRndCrossPromoItemCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var CrossPromoService = Engine.GetService<ICrossPromoService>();

            CrossPromoService.UnlockRandomItem();

            return UniTask.CompletedTask;
        }
    }
}
