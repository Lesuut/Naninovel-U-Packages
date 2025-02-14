namespace Naninovel.U.Reception.Commands
{
    [CommandAlias("reception")]
    public class ReceptionCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public IntegerParameter cardsCount;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ReceptionManager = Engine.GetService<IReceptionManager>();

            ReceptionManager.PlayReceptionMiniGame(cardsCount);

            return UniTask.CompletedTask;
        }
    }
}