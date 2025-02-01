using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Naninovel.UFlow.Editor
{
    using Elements;
    using System.Linq;
    using Enumeration;

    public class FlowGraphView : GraphView
    {
        public FlowGraphView() 
        {
            AddManipulators();
            AddGridBackground();

            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                if (startPort.portType != port.portType)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node", "FlowNode"));
            this.AddManipulator(CreateNodeContextualMenu("Add Node Test", "TestFlowNode"));
            this.AddManipulator(CreateNodeContextualMenu("Add Node Start", NodeType.Start));
            this.AddManipulator(CreateNodeContextualMenu("Add Node Waypoint", NodeType.Waypoint));
            this.AddManipulator(CreateNodeContextualMenu("Add Node End", NodeType.End));
            this.AddManipulator(CreateNodeContextualMenu("Add Node PlayScript", NodeType.PlayScript));
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, string ClassName)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(ClassName, actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, NodeType nodeType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode($"{nodeType}FlowNode", actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        public void CreateNode(NodeType nodeType, Vector2 position)
        {
            CreateNode($"{nodeType}FlowNode", position);
        }

        private FlowNode CreateNode(string ClassName, Vector2 position)
        {
            Type nodeType = Type.GetType($"Naninovel.UFlow.Elements.{ClassName}, Assembly-CSharp");

            FlowNode node = (FlowNode) Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            AddElement(node);

            return node;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet graphStyleSheet = (StyleSheet) EditorGUIUtility.Load("Naninovel_U_Flow/FlowViewStyles.uss");
            StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("Naninovel_U_Flow/FlowNodeStyles.uss");

            styleSheets.Add(graphStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        /// <summary>
        /// Получить список всех нодов в графе.
        /// </summary>
        public List<FlowNode> GetAllNodes()
        {
            return nodes.ToList().Cast<FlowNode>().ToList();
        }

        /// <summary>
        /// Получить список всех соединений (Edges) в графе.
        /// </summary>
        public List<Edge> GetAllEdges()
        {
            return edges.ToList();
        }
    }
}