using Unity.VisualScripting;

namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flow")]
    public class FlowCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            if (firstValue.Value.IsUnityNull())
            {
                FlowManager.StartFlow();
            }
            else
            {
                FlowManager.StartFlowByName(firstValue.Value);
            }

            return UniTask.CompletedTask;
        }
    }
}