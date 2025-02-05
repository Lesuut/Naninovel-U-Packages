using UnityEngine;
using UnityEngine.UI;
using Naninovel.UI;

namespace Naninovel.U.ParametersChoice
{
    public class ParametersChoiceUI : CustomUI
    {
        private IParametersChoiceManager ParametersChoiceManager;

        protected override void Awake()
        {
            base.Awake();

            ParametersChoiceManager = Engine.GetService<IParametersChoiceManager>();
        }

        /// <summary>
        /// Write the body for the ParametersChoice UI here
        /// </summary>
    }
}