using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Naninovel.UFlow.Utility
{
    using Data;
    using Naninovel.U.Flow;
    using System;
    using System.IO;
    using System.Linq;

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
        /// Сохраняет список нодов в ScriptableObject-ассет по указанному пути.
        /// </summary>
        /// <param name="path">Путь, по которому будет сохранен ассет.</param>
        /// <param name="nodes">Список нодов, который необходимо сохранить.</param>
        /// <remarks>
        /// Этот метод создает ScriptableObject, которое хранит список нодов.
        /// Он сериализует ноды и сохраняет их в ассет в проекте.
        /// Все типы нодов, включая наследуемые, сохраняются корректно.
        /// </remarks>
        public static void SaveNodesToAsset(string path, List<FlowNodeData> nodes)
        {
            // Создаем ассет, который будет хранить список нодов
            FlowAsset nodeAsset = ScriptableObject.CreateInstance<FlowAsset>();
            nodeAsset.flowNodeDatas = nodes;

            // Сохраняем ассет
            AssetDatabase.CreateAsset(nodeAsset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Nodes saved to asset: {path}");
        }

        /// <summary>
        /// Загружает и возвращает список нодов из указанного ассета.
        /// </summary>
        /// <param name="path">Путь, по которому находится ассет.</param>
        /// <returns>Список нодов, загруженных из ассета. Если ассет не найден, возвращает null.</returns>
        /// <remarks>
        /// Этот метод загружает ассет, который хранит список нодов, и восстанавливает их с учетом наследования.
        /// Если в ассете присутствуют наследуемые классы, они будут правильно десериализованы.
        /// </remarks>
        public static FlowAsset LoadNodesFromAsset(string path)
        {
            // Загружаем ассет по пути
            FlowAsset nodeAsset = AssetDatabase.LoadAssetAtPath<FlowAsset>(path);

            if (nodeAsset == null)
            {
                Debug.LogError("Failed to load node asset.");
                return null;
            }

            // Возвращаем список нодов
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
    }
}