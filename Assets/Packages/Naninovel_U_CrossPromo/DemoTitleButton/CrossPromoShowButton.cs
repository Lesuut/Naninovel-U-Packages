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

            // Жёстко отключаем кнопку, если кросс-промо не активен
            if (!crossPromoService.IsCrossPromoEnabled())
                mainButtonObj.SetActive(false);
        }

        protected override void OnButtonClick() => crossPromoService.ShowCrossPromo(LinkTransitionType.Menu);
    }
}