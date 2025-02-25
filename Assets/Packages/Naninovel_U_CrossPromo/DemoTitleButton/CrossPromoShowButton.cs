namespace Naninovel.U.CrossPromo
{
    public class CrossPromoShowButton : ScriptableButton
    {
        private ICrossPromoService crossPromoService;

        protected override void Awake()
        {
            base.Awake();
            crossPromoService = Engine.GetService<ICrossPromoService>();

            if (!crossPromoService.IsCrossPromoEnabled())
                gameObject.SetActive(false);
        }

        protected override void OnButtonClick() => crossPromoService.ShowCrossPromo(LinkTransitionType.Menu);
    }
}