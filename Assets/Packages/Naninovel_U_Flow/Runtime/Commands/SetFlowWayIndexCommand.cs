﻿namespace Naninovel.U.Flow.Commands
{
    [CommandAlias("setFlowWayIndex")]
    public class SetFlowWayIndexCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public IntegerParameter index;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var FlowManager = Engine.GetService<IFlowManager>();

            FlowManager.SetFlowWayIndex(index);

            return UniTask.CompletedTask;
        }
    }
}