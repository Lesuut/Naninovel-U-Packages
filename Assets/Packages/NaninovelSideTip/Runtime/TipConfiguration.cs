using System.Collections.Generic;
using UnityEngine;

namespace Naninovel.U.SideTip
{
    [EditInProjectSettings]
    public class TipConfiguration : Configuration
    {
        public string Category = "Tips";
        [Tooltip("Name in UI Configuration")]
        public string UIName = "SideTipUI";

        public List<TipContextKeyItem> ContextKeyItems = new List<TipContextKeyItem>();
    }
}