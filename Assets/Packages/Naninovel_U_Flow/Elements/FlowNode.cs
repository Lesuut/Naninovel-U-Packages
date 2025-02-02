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
        public int ID;
        private string selectedMapName; // Поле для хранения выбранного значения
        private PopupField<string> popupField;

        protected Port portAction;

        public virtual void Initialize(Vector2 position)
        {
            SetBaseStyle();
            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual FlowNodeData Serialization()
        {
            return new FlowNodeData()
            {
                NodeId = ID,
                NodePositionX = GetPosition().position.x,
                NodePositionY = GetPosition().position.y,
                NodeType = NodeType,
                MapName = selectedMapName // Сохранение выбранного значения
            };
        }

        public virtual void Deserialization(FlowNodeData flowNodeData)
        {
            NodeType = flowNodeData.NodeType;
            ID = flowNodeData.NodeId;
            SetPosition(new Rect(new Vector2(flowNodeData.NodePositionX, flowNodeData.NodePositionY), Vector2.zero));

            // Устанавливаем значение в popupField после его создания
            selectedMapName = flowNodeData.MapName;
            if (popupField != null)
                popupField.value = selectedMapName;

            SetBaseStyle();
        }

        protected virtual void SetBaseStyle()
        {
            title = $"{ID} Node";
        }

        public virtual void Draw()
        {
            TitleContainer();
            InputContainer();
            OutputContainer();
            ExtensionsContainer();

            RefreshExpandedState();
        }

        protected virtual void TitleContainer()
        {
            List<BackgroundItem> backgroundItems = FlowUtility.GetAllMaps();
            List<string> mapNames = backgroundItems.Select(item => item.Name).ToList();

            if (mapNames.Count > 0 && string.IsNullOrEmpty(selectedMapName))
                selectedMapName = mapNames[0]; // Устанавливаем первое значение списка по умолчанию

            popupField = new PopupField<string>("Map ->", mapNames, selectedMapName);
            popupField.AddToClassList("popup-field-map");

            popupField.RegisterValueChangedCallback(evt =>
            {
                selectedMapName = evt.newValue;

                var oldImage = titleContainer.contentContainer.Query<Image>().First();
                if (oldImage != null)
                    oldImage.RemoveFromHierarchy();

                BackgroundItem selectedItem = backgroundItems.FirstOrDefault(item => item.Name == evt.newValue);

                if (selectedItem.Icone != null)
                {
                    var nodeImage = new Image();
                    nodeImage.image = selectedItem.Icone;
                    nodeImage.AddToClassList("node-image");
                    titleContainer.contentContainer.Add(nodeImage);
                }
            });

            titleContainer.contentContainer.Add(popupField);
        }

        protected virtual void InputContainer() { }
        protected virtual void OutputContainer()
        {
            portAction = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Action));
            portAction.portName = "Actions";
            portAction.AddToClassList("port-action");
            outputContainer.Add(portAction);
        }
        protected virtual void ExtensionsContainer() { }
    }
}