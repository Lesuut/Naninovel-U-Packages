using Naninovel.UI;
using System.Collections.Generic;
using System.Linq;

namespace Naninovel.U.CG
{
    public class UCGGalleryPanel : CustomUI
    {
        private List<IUpdatedCGSlot> updatedCGSlots = new List<IUpdatedCGSlot>();

        protected override void OnEnable()
        {
            base.OnEnable();

            updatedCGSlots = GetComponentsInChildren<IUpdatedCGSlot>().ToList();

            updatedCGSlots.ForEach(updatedCGSlot => { updatedCGSlot.Init(); });
        }

        public override void Show()
        {
            base.Show();

            updatedCGSlots.ForEach(updatedCGSlot => { updatedCGSlot.SlotUpdate(); });
        }
    }
}
