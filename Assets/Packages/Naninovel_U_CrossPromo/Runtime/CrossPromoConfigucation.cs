using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    /// <summary>
    /// Contains configuration data for the CrossPromo systems.
    /// </summary>  

    [EditInProjectSettings]
    public class CrossPromoConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "CrossPromo";

        [TextArea]
        
        public string GoogleSheetDataURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRPHeIeKCH1DtQFJ9fwZEDvSNoWHj-FgrV2-YeT9oDN2_Q6GBVHAVCjtOgjo5yv7vwHDcIGNCAxWgi3/pub?gid=0&single=true&output=csv";
        [Space]
        public bool debug = false;
        public bool crossPromoEnable = true;

        [Space]
        public UnlockableImages[] unlockableImages = new UnlockableImages[]
        {
            new UnlockableImages("CrossPromoUnlockableItem_0"),
            new UnlockableImages("CrossPromoUnlockableItem_1"),
            new UnlockableImages("CrossPromoUnlockableItem_2"),
            new UnlockableImages("CrossPromoUnlockableItem_3"),
            new UnlockableImages("CrossPromoUnlockableItem_4"),
            new UnlockableImages("CrossPromoUnlockableItem_5")
        };

        public string achievementNaniCommand = "@ach";
    }
}