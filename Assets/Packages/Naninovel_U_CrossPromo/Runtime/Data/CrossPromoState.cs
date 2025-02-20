using System;
using System.Collections.Generic;

namespace Naninovel.U.CrossPromo
{
    [Serializable]
    public class CrossPromoState
    {
        public List<int> availableIdSlots;
        public List<int> viewedIdSlots;

        public CrossPromoState()
        {
            availableIdSlots = new List<int>();
            viewedIdSlots = new List<int>();

            availableIdSlots.Add(0);
        }

        public CrossPromoState(CrossPromoState other)
        {
            availableIdSlots = other.availableIdSlots;
            viewedIdSlots= other.viewedIdSlots;
        }
    }
}
