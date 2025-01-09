using Naninovel.UI;

namespace Naninovel.U.Base
{
    public class ABaseUI : CustomUI
    {
        private IBaseAService baseService;

        protected override void Awake()
        {
            base.Awake();

            baseService = Engine.GetService<IBaseAService>();
        }

        /// <summary>
        /// Write the body for the Base UI here
        /// </summary>
    }
}