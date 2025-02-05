using Naninovel;

[CommandAlias("test")]
public class TestCommand : Command, Command.ILocalizable
{
    [ParameterAlias(NamelessParameterAlias), LocalizableParameter]
    public StringParameter firstValue;

    /*[ParameterAlias("id"), LocalizableParameter]
    public StringParameter text = "Hello World!";*/

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {      
        UnityEngine.Debug.Log(firstValue.Value);
        return UniTask.CompletedTask;
    }
}
