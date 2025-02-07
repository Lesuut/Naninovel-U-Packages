using UnityEngine;

namespace Naninovel.U.ChoiceBox
{
    public interface IChoiceBoxService : IEngineService
    {
        public void AddOption(string title, string toDo);
        public void SetTitle(string title);
        public void ShowChoiceBox();
    }
}