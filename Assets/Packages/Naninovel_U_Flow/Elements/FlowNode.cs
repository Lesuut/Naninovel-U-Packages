using Naninovel.UFlow.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Naninovel.UFlow.Elements
{
    using Enumeration;
    using Naninovel.U.Flow;
    using Naninovel.UFlow.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
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
            // Получаем список всех элементов
            List<BackgroundItem> backgroundItems = FlowUtility.GetAllMaps();

            // Создаем элемент для выпадающего списка
            var popupField = new PopupField<string>("Map ->", backgroundItems.Select(item => item.Name).ToList(), 0);
            popupField.AddToClassList("popup-field-map"); // Применяем стиль

            // Обработчик изменения значения в выпадающем списке
            popupField.RegisterValueChangedCallback(evt =>
            {
                // Удаляем старое изображение (если есть)
                var oldImage = titleContainer.contentContainer.Query<Image>().First();  // Получаем первое изображение
                if (oldImage != null)
                    oldImage.RemoveFromHierarchy();

                // Ищем выбранный элемент по имени
                BackgroundItem selectedItem = backgroundItems.FirstOrDefault(item => item.Name == evt.newValue);

                if (selectedItem.Icone != null && selectedItem.Icone != null)
                {
                    // Создаем новый элемент для изображения
                    var nodeImage = new Image();
                    nodeImage.image = selectedItem.Icone;
                    nodeImage.AddToClassList("node-image"); // Применяем стиль
                    titleContainer.contentContainer.Add(nodeImage); // Добавляем изображение в контейнер
                }
            });

            // Добавляем выпадающий список в контейнер
            titleContainer.contentContainer.Add(popupField);
        }

        protected virtual void InputContainer() { }
        protected virtual void OutputContainer() 
        { 
            Port portAction = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Action));
            portAction.portName = $"Actions";
            portAction.AddToClassList("port-action");
            outputContainer.Add(portAction);
        }      
        protected virtual void ExtensionsContainer() { }
    }
}