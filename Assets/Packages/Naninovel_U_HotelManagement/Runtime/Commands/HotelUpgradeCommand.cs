namespace Naninovel.U.HotelManagement.Commands
{
    [CommandAlias("hotelUpgrade")]
    public class HotelUpgradeCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var HotelManagementManager = Engine.GetService<IHotelManagementManager>();

            HotelManagementManager.Improve(firstValue.Value);

            return UniTask.CompletedTask;
        }
    }
}
