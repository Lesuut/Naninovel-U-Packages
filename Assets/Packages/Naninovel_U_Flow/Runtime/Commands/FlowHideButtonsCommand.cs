namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flowButtonsHide")]
    public class FlowHideButtonsCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public BooleanParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetButtonsHideStatus(firstValue);

            return UniTask.CompletedTask;
        }
    }
}
