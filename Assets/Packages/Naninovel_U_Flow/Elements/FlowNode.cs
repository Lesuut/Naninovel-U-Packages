using Naninovel.UFlow.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using Naninovel.UFlow.Utility;
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
            var popupField = new PopupField<string>("Map ->", FlowUtility.GetAllMaps(), 0); // Список строк и индекс по умолчанию
            popupField.AddToClassList("popup-field-map"); // Применяем стиль
            titleContainer.contentContainer.Add(popupField);
        }
        protected virtual void InputContainer() { }
        protected virtual void OutputContainer() { }      
        protected virtual void ExtensionsContainer() { }
    }
}