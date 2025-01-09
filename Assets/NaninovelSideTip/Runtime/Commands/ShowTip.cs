namespace Naninovel.U.SideTip
{
    [CommandAlias("showTip")]
    public class ShowTip : Command
    {
        [RequiredParameter, ParameterAlias("key")]
        public StringParameter ItemKey;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();

            var uiTip = uiManager.GetUI<TipUI>();

            if (!uiTip.Visible) uiTip.Show();

            uiTip.ShowTip(ItemKey);

            return UniTask.CompletedTask;
        }
    }
}