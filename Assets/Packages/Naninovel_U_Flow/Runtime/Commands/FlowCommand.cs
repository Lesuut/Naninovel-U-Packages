namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flow")]
    public class FlowCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.StartFlow();

            return UniTask.CompletedTask;
        }
    }
}