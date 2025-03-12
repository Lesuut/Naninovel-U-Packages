using Naninovel.UI;
using System.Collections.Generic;
using System.Linq;

namespace Naninovel.U.CG
{
    public class UCGGalleryPanel : CustomUI
    {
        private List<IUpdatedCGSlot> updatedCGSlots = new List<IUpdatedCGSlot>();

        private bool init = false;

        public override void Show()
        {
            base.Show();

            if (!init)
            {
                updatedCGSlots = GetComponentsInChildren<IUpdatedCGSlot>().ToList();

                updatedCGSlots.ForEach(updatedCGSlot => { updatedCGSlot.Init(); });

                init = true;
            }

            updatedCGSlots.ForEach(updatedCGSlot => { updatedCGSlot.SlotUpdate(); });
        }
    }
}
