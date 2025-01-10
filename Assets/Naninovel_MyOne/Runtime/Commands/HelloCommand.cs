namespace Naninovel.U.MyOne
{
    [CommandAlias("hello")]
    public class HelloCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var MyOneService = Engine.GetService<IMyOneService>();

            return UniTask.CompletedTask;
        }
    }
}