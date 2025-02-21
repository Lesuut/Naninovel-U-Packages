using Naninovel;
using Naninovel.U.CrossPromo;

public class CrossPromoShowButton : ScriptableButton
{
    private ICrossPromoService crossPromoService;

    protected override void Awake()
    {
        base.Awake();

        crossPromoService = Engine.GetService<ICrossPromoService>();
    }

    protected override void OnButtonClick() => crossPromoService.ShowCrossPromo(LinkTransitionType.Menu);
}
