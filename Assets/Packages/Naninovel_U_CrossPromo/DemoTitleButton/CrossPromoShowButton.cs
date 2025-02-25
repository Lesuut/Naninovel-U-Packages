using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    public class CrossPromoShowButton : ScriptableButton
    {
        [SerializeField] private GameObject mainButtonObj;

        private ICrossPromoService crossPromoService;

        protected override void Awake()
        {
            base.Awake();
            crossPromoService = Engine.GetService<ICrossPromoService>();

            if (!crossPromoService.IsCrossPromoEnabled())
                mainButtonObj.SetActive(false);
        }

        protected override void OnButtonClick() => crossPromoService.ShowCrossPromo(LinkTransitionType.Menu);
    }
}