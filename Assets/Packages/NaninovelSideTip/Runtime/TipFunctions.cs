namespace Naninovel.U.SideTip
{
    [ExpressionFunctions]
    public static class TipFunctions
    {
        public static string GetCurrentTipKey()
        {
            var uiManager = Engine.GetService<IUIManager>();
            var tip = uiManager.GetUI<TipUI>();
            return tip.CurrentKey;
        }
        public static bool IsCurrentTipKey(string tipKey)
        {
            return GetCurrentTipKey() == tipKey;
        }
    }
}