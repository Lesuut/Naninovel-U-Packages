namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("setFlowWayIndex")]
    public class SetFlowWayIndexCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public IntegerParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetFlowWayIndex(firstValue);

            return UniTask.CompletedTask;
        }
    }
}