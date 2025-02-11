namespace Naninovel.U.Puzzle.Commands
{
    [CommandAlias("puzzle")]
    public class PuzzleCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter firstValue;

        /*[ParameterAlias("id"), LocalizableParameter]
        public StringParameter text = "Hello World!";*/

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            return UniTask.CompletedTask;
        }
    }
}