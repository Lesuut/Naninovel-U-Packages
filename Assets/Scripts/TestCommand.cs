using Naninovel;

[CommandAlias("test")]
public class TestCommand : Command, Command.ILocalizable
{
    /*[ParameterAlias(NamelessParameterAlias), LocalizableParameter]
    public StringParameter firstValue;*/

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        //UnityEngine.Debug.Log(Engine.GetService<ISmartQuestService>().GetQuestsTextInfo());
        return UniTask.CompletedTask;
    }
}
