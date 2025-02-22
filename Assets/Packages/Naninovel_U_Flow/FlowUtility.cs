﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Naninovel.UFlow.Utility
{
    using Data;
    using Naninovel.U.Flow;
    using Naninovel.UFlow.Elements;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;

    public static class FlowUtility
    {
        /// <summary>
        /// Загружает первый найденный ScriptableObject указанного типа из проекта.
        /// </summary>
        /// <typeparam name="T">Тип ScriptableObject, который нужно загрузить.</typeparam>
        /// <returns>Загруженный ScriptableObject или null, если объект не найден.</returns>
        public static T LoadScriptableObject<T>() where T : ScriptableObject
        {
            // Ищем все файлы с указанным типом
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            if (guids.Length == 0)
            {
                Debug.LogError($"No ScriptableObject of type {typeof(T).Name} found in the project.");
                return null;
            }

            // Если найдено больше одного, выводим предупреждение
            if (guids.Length > 1)
            {
                Debug.LogWarning($"Multiple ScriptableObjects of type {typeof(T).Name} found. Using the first one.");
            }

            // Берем первый найденный файл
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            T scriptableObject = AssetDatabase.LoadAssetAtPath<T>(path);

            if (scriptableObject != null)
            {
                return scriptableObject;
            }
            else
            {
                Debug.LogError($"Failed to load ScriptableObject of type {typeof(T).Name} at path: {path}");
                return null;
            }
        }


        /// <summary>
        /// Сохраняет список нодов в ScriptableObject-ассет с сериализацией в JSON.
        /// </summary>
        public static void SaveNodesToAsset(string path, List<FlowNodeData> nodes)
        {
            FlowAsset nodeAsset = ScriptableObject.CreateInstance<FlowAsset>();

            nodeAsset.flowNodeDatas = nodes;
            nodeAsset.SaveData(); // Сохраняем данные через SaveData

            AssetDatabase.CreateAsset(nodeAsset, path);
            EditorUtility.SetDirty(nodeAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Nodes saved to asset: {path}");
        }

        /// <summary>
        /// Загружает и десериализует список нодов из ассета.
        /// </summary>
        public static FlowAsset LoadNodesFromAsset(string path)
        {
            FlowAsset nodeAsset = AssetDatabase.LoadAssetAtPath<FlowAsset>(path);

            Debug.Log($"{nodeAsset} path: {path}");

            if (nodeAsset == null)
            {
                Debug.LogError($"Failed to load node asset at path: {path}");
                return null;
            }

            nodeAsset.LoadData(); // Загружаем данные через LoadData

            Debug.Log($"Successfully loaded FlowAsset from: {path}, nodes count: {nodeAsset.flowNodeDatas?.Count ?? 0}");
            return nodeAsset;
        }

        public static List<BackgroundItem> GetAllMaps()
        {
            return LoadScriptableObject<FlowConfiguration>().Backgrounds.ToList();
        }
        public static List<string> GetAllButtons()
        {
            return LoadScriptableObject<FlowConfiguration>().TransferButtons.Select(item => item.Name).ToList();
        }

        /// <summary>
        /// Ищет все файлы с расширением .nani в проекте и возвращает их имена.
        /// </summary>
        public static List<string> GetAllNaniFiles()
        {
            string[] files = Directory.GetFiles(Application.dataPath, "*.nani", SearchOption.AllDirectories);
            return files.Select(Path.GetFileName).ToList();
        }

        /// <summary>
        /// Сериализует подключения нодов и возвращает список связанных нодов.
        /// </summary>
        /// <summary>
        /// Сериализует подключения нодов (входных или выходных портов) и возвращает список связанных нодов.
        /// </summary>
        public static List<FlowPortData> SerializeFlowNodeConnections(VisualElement container, bool isOutputPort)
        {
            List<FlowPortData> connectedNodeIds = new List<FlowPortData>();

            // Проходим по всем портам в контейнере
            foreach (var port in container.Children())
            {
                if (port is Port portElement)
                {
                    bool hasConnections = false;

                    // В зависимости от флага обрабатываем либо выходной, либо входной порт
                    foreach (var edge in portElement.connections)
                    {
                        if (isOutputPort)
                        {
                            if (edge.input.node is FlowNode connectedNode)
                            {
                                // Добавляем данные порта с NodeId и PortId
                                connectedNodeIds.Add(new FlowPortData()
                                {
                                    NodeId = connectedNode.ID,
                                    PortId = container.IndexOf(port) // Порядковый номер порта
                                });
                                hasConnections = true;
                            }
                        }
                        else
                        {
                            if (edge.output.node is FlowNode connectedNode)
                            {
                                // Добавляем данные порта с NodeId и PortId
                                connectedNodeIds.Add(new FlowPortData()
                                {
                                    NodeId = connectedNode.ID,
                                    PortId = container.IndexOf(port) // Порядковый номер порта
                                });
                                hasConnections = true;
                            }
                        }
                    }

                    // Если у данного порта нет соединений, добавляем -1 для NodeId и сохраняем PortId
                    if (!hasConnections)
                    {
                        connectedNodeIds.Add(new FlowPortData()
                        {
                            NodeId = -1,
                            PortId = container.IndexOf(port) // Порядковый номер порта
                        });
                    }
                }
            }

            return connectedNodeIds;
        }

        public static FlowNode[] GetConnectedNodes(Port port)
        {
            // Список для хранения всех подключенных узлов
            List<FlowNode> connectedNodes = new List<FlowNode>();

            // Проходим по всем соединениям, связанным с этим портом
            foreach (var edge in port.connections)
            {
                // Получаем целевой порт
                var targetPort = edge.input;  // Для входных соединений используем edge.input, для выходных edge.output

                // Проверяем, если порт подключен, то добавляем его узел в список
                if (targetPort != null)
                {
                    var connectedNode = targetPort.node as FlowNode;
                    if (connectedNode != null)
                    {
                        connectedNodes.Add(connectedNode);
                    }
                }
            }

            // Возвращаем массив узлов
            return connectedNodes.ToArray();
        }

    }
}