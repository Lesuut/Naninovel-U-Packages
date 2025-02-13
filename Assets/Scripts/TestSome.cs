using UnityEngine;
using Naninovel;

public class TestSome : MonoBehaviour
{
    void OnEnable()
    {
       /* var ILocalizationManager = Engine.GetService<ILocalizationManager>();
        var textManager = Engine.GetService<ITextManager>();
        ILocalizationManager.OnLocaleChanged += (string locate) => Debug.Log(textManager.GetRecordValue("Reception.Screen.DateOfBirth.0", "Reception"));*/

        TestPrinter();
    }

    private void TestPrinter()
    {
        Debug.Log("TestPrinter");
        var textPrinterManager = Engine.GetService<ITextPrinterManager>();
        textPrinterManager.PrintTextAsync("ReceptionPrinter", "123", "Custom");
    }
}
