using Naninovel.UFlow.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using UnityEngine.UIElements;

    public class FlowNode : Node
    {
        public NodeType NodeType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            SetBaseStyle();
            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual FlowNodeData Serialization()
        {
            return new FlowNodeData()
            {
                NodePosition = GetPosition().position,
                NodeType = NodeType
            };
        }
        public virtual void Deserialization(FlowNodeData flowNodeData)
        {
            SetPosition(new Rect(flowNodeData.NodePosition, Vector2.zero));
        }

        protected virtual void SetBaseStyle()
        {
            title = "Node";
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

        protected virtual void TitleContainer() 
        {
            EnumField enumField = new EnumField(NodeType.Start);
            enumField.AddToClassList("enum-field"); // Применяем стиль
            titleContainer.Add(enumField);
        }
        protected virtual void InputContainer() { }
        protected virtual void OutputContainer() { }      
        protected virtual void ExtensionsContainer() { }
    }
}