using UnityEngine;
using UnityEngine.UI;
using Naninovel.UI;

namespace Naninovel.U.CrossPromo
{
    public class CrossPromoUI : CustomUI
    {
        private ICrossPromoService CrossPromoService;

        protected override void Awake()
        {
            base.Awake();

            CrossPromoService = Engine.GetService<ICrossPromoService>();
        }

        /// <summary>
        /// Write the body for the CrossPromo UI here
        /// </summary>
    }
}