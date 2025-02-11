namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowSetCustomEnd")]
    public class SetCustomEndFlowCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public IntegerParameter id;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetCustomFLowEndID(id);

            return UniTask.CompletedTask;
        }
    }
}