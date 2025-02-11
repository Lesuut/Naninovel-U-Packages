using System;
using System.Collections.Generic;

namespace Naninovel.U.UIInteractionToolkit
{
    [Serializable]
    public class UIInteractionToolkitState
    {
        public List<string> Value;

        public UIInteractionToolkitState()
        {
            // Initialization Values
            Value = new List<string>
            {
               "Value1",
               "Value2",
               "Value3"
            };
        }

        public UIInteractionToolkitState(UIInteractionToolkitState other)
        {
            // Load and set Data
            Value = new List<string>(other.Value);
        }
    }
}
