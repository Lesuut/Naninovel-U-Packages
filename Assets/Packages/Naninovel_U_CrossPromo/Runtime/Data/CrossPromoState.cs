using System;
using System.Collections.Generic;

namespace Naninovel.U.CrossPromo
{
    [Serializable]
    public class CrossPromoState
    {
        public List<int> availableIdSlots;
        public List<int> receivedIdSlots;

        public CrossPromoState()
        {
            availableIdSlots = new List<int>();
            receivedIdSlots = new List<int>();
        }

        public CrossPromoState(CrossPromoState other)
        {
            if (other != null)
            {
                availableIdSlots = other.availableIdSlots;
                receivedIdSlots = other.receivedIdSlots;
            }
            else
            {
                availableIdSlots = new List<int>();
                receivedIdSlots = new List<int>();
            }
        }
    }
}
