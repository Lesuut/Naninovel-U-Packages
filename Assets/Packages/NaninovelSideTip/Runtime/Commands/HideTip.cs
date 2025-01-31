namespace Naninovel.U.SideTip
{
    [CommandAlias("hideTip")]
    public class HideTip : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();

            var uiTip = uiManager.GetUI<TipUI>();

            if (uiTip.Visible) uiTip.HideTip(); uiTip.Hide();

            return UniTask.CompletedTask;
        }
    }
}