namespace Naninovel.U.Parameters.Commands
{
    [CommandAlias("setparameter")]
    public class SetParameterCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        [ParameterAlias("value")]
        public IntegerParameter value = 0;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var ParametersManager = Engine.GetService<IParametersManager>();

            if (value != 0)
            {
                ParametersManager.SetParametrOperation(firstValue, value);
            }

            return UniTask.CompletedTask;
        }
    }
}