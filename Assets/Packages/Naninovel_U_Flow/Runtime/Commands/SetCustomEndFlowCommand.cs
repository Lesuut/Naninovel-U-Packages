namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowSetCustomEnd")]
    public class SetCustomEndFlowCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter endBackground;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetCustomFLowEndBack(endBackground.Value);

            return UniTask.CompletedTask;
        }
    }
}