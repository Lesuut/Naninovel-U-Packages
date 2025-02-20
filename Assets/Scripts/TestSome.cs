using UnityEngine;
using Naninovel;
using Naninovel.U.CrossPromo;
using System.Collections;

public class TestSome : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(cur());
    }
    private IEnumerator cur()
    {
        yield return new WaitForSeconds(1f);
        Engine.GetService<IUIManager>().GetUI<CrossPromoUI>().Show();
    }
}
