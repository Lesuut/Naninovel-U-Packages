using UnityEngine;
using UnityEngine.UI;
using Naninovel.UI;

namespace Naninovel.U.Reception
{
    public class ReceptionUI : CustomUI
    {
        private IReceptionManager ReceptionManager;

        protected override void Awake()
        {
            base.Awake();

            ReceptionManager = Engine.GetService<IReceptionManager>();
        }

        /// <summary>
        /// Write the body for the Reception UI here
        /// </summary>
    }
}