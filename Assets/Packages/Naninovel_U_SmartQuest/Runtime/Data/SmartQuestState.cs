using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Naninovel.U.SmartQuest
{
    [Serializable]
    public class SmartQuestState
    {
        // Не сериализуемый список квестов
        [NonSerialized]
        public List<Quest> Quests;

        public string jsonData;

        public SmartQuestState()
        {
            Quests = new List<Quest>();
        }

        public SmartQuestState(SmartQuestState other)
        {
            jsonData = other.jsonData;
        }

        // Метод для сохранения данных о квестах в строку
        public void SaveData()
        {
            // Сериализуем данные в строку с полиморфизмом
            jsonData = JsonConvert.SerializeObject(Quests, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All // Включаем полиморфизм
                });

            //Debug.Log($"Save:\n:{jsonData}");
        }

        // Метод для загрузки данных из строки
        public void LoadData()
        {
            try
            {
                if (string.IsNullOrEmpty(jsonData))
                {
                    // Если строка пуста, ничего не загружаем
                    return;
                }

                // Десериализуем данные из строки в список квестов
                Quests = JsonConvert.DeserializeObject<List<Quest>>(jsonData,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All // Поддержка полиморфизма при десериализации
                    });

                if (Quests == null)
                {
                    Debug.LogWarning("Deserialized Quests is null.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deserializing Quests: {e.Message}");
            }

            //Debug.Log($"Load:\n:{jsonData}");
        }
    }
}