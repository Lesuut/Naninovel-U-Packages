namespace Naninovel.U.Parameters
{
    [ExpressionFunctions]
    public static class ParametersFunctions
    {
        public static float GetParameter(string key)
        {
            var ParametersManager = Engine.GetService<IParametersManager>();

            return ParametersManager.GetParametrOperation(key);
        }
    }
}