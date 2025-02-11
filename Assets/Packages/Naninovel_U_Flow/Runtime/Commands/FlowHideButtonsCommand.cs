namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowButtonsHide")]
    public class FlowHideButtonsCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public BooleanParameter firstValue;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetButtonsHideStatus(firstValue);

            return UniTask.CompletedTask;
        }
    }
}
