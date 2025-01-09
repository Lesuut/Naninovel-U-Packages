namespace Naninovel.U.SideTip
{
    [CommandAlias("tip")]
    public class FastTip : Command
    {
        [ParameterAlias("key")]
        public StringParameter ItemKey = "";

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<IUIManager>();

            var uiTip = uiManager.GetUI<TipUI>();

            if (ItemKey.Value == "")
            {
                if (uiTip.Visible) uiTip.HideTip(); uiTip.Hide();
            }
            else
            {
                if (!uiTip.Visible) uiTip.Show();
                uiTip.ShowTip(ItemKey);
            }

            return UniTask.CompletedTask;
        }
    }
}
