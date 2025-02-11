namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowPause")]
    public class PauseFlowCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetButtonsHideStatus(true);

            return UniTask.CompletedTask;
        }
    }
}
