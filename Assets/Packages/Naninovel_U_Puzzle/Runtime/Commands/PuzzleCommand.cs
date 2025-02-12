namespace Naninovel.U.Puzzle.Commands
{
    [CommandAlias("puzzle")]
    public class PuzzleCommand : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter firstValue;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            Engine.GetService<IUIManager>().GetUI<PuzzleUI>().StartPuzzleMiniGame(firstValue);

            return UniTask.CompletedTask;
        }
    }
}