using Naninovel.UI;
using UnityEngine.UI;

namespace Naninovel.U.Flow
{
    public class TransitionButtonUI : CustomUI
    {
        public Button TransitionButton;

        public void Destroy() { Destroy(gameObject); }
    }
}
