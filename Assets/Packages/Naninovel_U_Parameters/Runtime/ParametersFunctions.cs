namespace Naninovel.U.Parameters
{
    [ExpressionFunctions]
    public static class ParametersFunctions
    {
        public static float GetParameter(string key)
        {
            var ParametersManager = Engine.GetService<IParametersManager>();

            UnityEngine.Debug.Log($"{key}: {ParametersManager.GetParametrOperation(key)}");

            return ParametersManager.GetParametrOperation(key);
        }
    }
}