using UnityEngine;

namespace Naninovel.U.Parameters
{
    public interface IParametersManager : IEngineService
    {
        public void SetParametrOperation(string key, int value);
        public float GetParametrOperation(string key);
    }
}