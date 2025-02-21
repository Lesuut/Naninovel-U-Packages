namespace Naninovel.U.CrossPromo.Commands
{
    [CommandAlias("crossPromo")]
    public class OpenCrossPromoCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var CrossPromoService = Engine.GetService<ICrossPromoService>();

            CrossPromoService.ShowCrossPromo(LinkTransitionType.Final);

            return UniTask.CompletedTask;
        }
    }
}