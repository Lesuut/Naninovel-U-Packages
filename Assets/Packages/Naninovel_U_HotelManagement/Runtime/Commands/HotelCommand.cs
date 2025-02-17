namespace Naninovel.U.HotelManagement.Commands
{
    [CommandAlias("hotel")]
    public class HotelCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var HotelManagementManager = Engine.GetService<IHotelManagementManager>();

            return UniTask.CompletedTask;
        }
    }
}