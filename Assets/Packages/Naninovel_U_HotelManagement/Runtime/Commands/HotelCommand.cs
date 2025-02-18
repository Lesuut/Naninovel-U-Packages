namespace Naninovel.U.HotelManagement.Commands
{
    [CommandAlias("hotel")]
    public class HotelCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var HotelManagementManager = Engine.GetService<IHotelManagementManager>();

            HotelManagementManager.StartMiniGame(firstValue);

            return UniTask.CompletedTask;
        }
    }
}