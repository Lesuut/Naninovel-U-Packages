namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowStop")]
    public class StopFlowCommand : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            UnityEngine.Debug.Log("stopFlow");

            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.StopFlow();

            return UniTask.CompletedTask;
        }
    }
}
