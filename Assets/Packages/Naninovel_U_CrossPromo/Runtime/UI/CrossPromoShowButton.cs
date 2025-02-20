using Naninovel;
using Naninovel.U.CrossPromo;

public class CrossPromoShowButton : ScriptableButton
{
    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();

        uiManager = Engine.GetService<IUIManager>();
    }

    protected override void OnButtonClick() => uiManager.GetUI<CrossPromoUI>()?.Show();
}
