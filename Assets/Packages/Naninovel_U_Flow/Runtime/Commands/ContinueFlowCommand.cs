namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowContinue")]
    public class ContinueFlowCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetButtonsHideStatus(false);

            return UniTask.CompletedTask;
        }
    }
}
