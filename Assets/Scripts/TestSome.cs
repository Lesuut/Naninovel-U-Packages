using UnityEngine;
using Naninovel;
using Naninovel.U.Reception;

public class TestSome : MonoBehaviour
{
    void Start()
    {
        var ILocalizationManager = Engine.GetService<ILocalizationManager>();
        var textManager = Engine.GetService<ITextManager>();
        ILocalizationManager.OnLocaleChanged += (string locate) => Debug.Log(textManager.GetRecordValue("Reception.Screen.DateOfBirth.0", "Reception"));
    }

    private void TestPrinter()
    {
        var textPrinterManager = Engine.GetService<ITextPrinterManager>();
        textPrinterManager.PrintTextAsync("Dialogue", "123", "Custom");
    }
}
