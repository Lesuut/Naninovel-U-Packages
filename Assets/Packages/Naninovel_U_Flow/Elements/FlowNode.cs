using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    public class FlowNode : Node
    {
        public virtual void Initialize(Vector2 position)
        {
            SetBaseStyle();
            SetPosition(new Rect(position, Vector2.zero));
        }

        protected virtual void SetBaseStyle()
        {
            title = "Node";
            mainContainer.AddToClassList("flow-node");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */
            TitleContainer();

            /* INPUT CONTAINER */
            InputContainer();

            /* OUTPUT CONTAINER */
            OutputContainer();

            /* EXTENSIONS CONTAINER */
            ExtensionsContainer();

            RefreshExpandedState();
        }

        protected virtual void TitleContainer() { }
        protected virtual void InputContainer() { }
        protected virtual void OutputContainer() { }      
        protected virtual void ExtensionsContainer() { }
    }
}