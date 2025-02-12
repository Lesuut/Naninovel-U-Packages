using UnityEngine;
using Naninovel;
using Naninovel.U.Reception;

public class TestSome : MonoBehaviour
{
    void Start()
    {
        Debug.Log(string.Join(", ", Engine.GetConfiguration<ReceptionConfiguration>().GetNames()));
    }
}
