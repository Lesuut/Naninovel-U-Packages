using Naninovel.UFlow.Data;
using UnityEngine;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "NewFlowNodeAsset", menuName = "FlowGraph/FlowNodeAsset")]
public class FlowAsset : ScriptableObject
{
    [SerializeField, TextArea(5, 1000)] private string jsonData;

    public string JsonData
    {
        get => jsonData;
        set => jsonData = value;
    }

    [NonSerialized] public List<FlowNodeData> flowNodeDatas = new List<FlowNodeData>();

    public void SaveData()
    {
        JsonData = JsonConvert.SerializeObject(flowNodeDatas, Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All, // Включаем полиморфизм
            });
    }

    public void LoadData()
    {
        try
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                //Debug.LogWarning("jsonData is null or empty. Unable to load data.");
                return;
            }

            // Десериализуем с типами, поддерживающими полиморфизм
            flowNodeDatas = JsonConvert.DeserializeObject<List<FlowNodeData>>(jsonData,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            if (flowNodeDatas == null)
            {
                Debug.LogWarning("Deserialized flowNodeDatas is null.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deserializing FlowNodeData: {e.Message}");
        }
    }
}