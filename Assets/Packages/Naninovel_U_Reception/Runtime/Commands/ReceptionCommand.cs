namespace Naninovel.U.Reception.Commands
{
    [CommandAlias("reception")]
    public class ReceptionCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ReceptionManager = Engine.GetService<IReceptionManager>();

            return UniTask.CompletedTask;
        }
    }
}