namespace Naninovel.U.CG
{
    public class UCGButton : ScriptableButton
    {
        private IUIManager uiManager;

        protected override void Awake()
        {
            base.Awake();

            uiManager = Engine.GetService<IUIManager>();
        }

        protected override void OnButtonClick() => uiManager.GetUI<UCGGalleryPanel>()?.Show();
    }
}