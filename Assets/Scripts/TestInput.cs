using UnityEngine;
using Naninovel;

public class TestInput : MonoBehaviour
{
    void Start()
    {
        Engine.GetService<IInputManager>().GetSkip().OnStart += () => Debug.Log("Skip");
        Engine.GetService<IInputManager>().GetCancel().OnStart += () => Debug.Log("Cancel");
        Engine.GetService<IInputManager>().GetSampler("My").OnStart += () => Debug.Log("My");
    }
}
