using Unity.VisualScripting;

namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("flow")]
    public class FlowCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter assetName;

        [ParameterAlias("back"), ParameterDefaultValue("")]
        public StringParameter startBackground;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            if (assetName.Value.IsUnityNull())
            {
                FlowManager.StartFlow();
            }
            else
            {
                FlowManager.StartFlowByName(assetName.Value, startBackground);
            }

            return UniTask.CompletedTask;
        }
    }
}