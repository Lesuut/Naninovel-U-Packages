using Naninovel.UI;

namespace Naninovel.Commands
{
    /// <summary>
    /// Stops the currently showing toast notification immediately.
    /// </summary>
    [CommandAlias("stopToast")]
    public class StopToastUI : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var toastUI = Engine.GetService<IUIManager>().GetUI<IToastUI>();
            toastUI?.HideToastImmediately();  // Скрываем тост немедленно
            return UniTask.CompletedTask;
        }
    }
}