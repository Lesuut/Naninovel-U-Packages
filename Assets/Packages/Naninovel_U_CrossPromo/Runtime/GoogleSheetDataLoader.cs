using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Naninovel.U.CrossPromo
{
    public class GoogleSheetDataLoader
    {
        public struct SheetData
        {
            public Sprite Image;
            public string Url;
        }

        public async Task<SheetData[]> LoadDataAsync(string googleSheetUrl)
        {
            string[] lines = await LoadDataFromGoogleSheet(googleSheetUrl);
            if (lines == null || lines.Length == 0)
                return Array.Empty<SheetData>();

            return await ParseLinesToSheetData(lines);
        }

        private async Task<string[]> LoadDataFromGoogleSheet(string googleSheetUrl)
        {
            using UnityWebRequest request = UnityWebRequest.Get(googleSheetUrl);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Ошибка загрузки данных: " + request.error);
                return null;
            }

            return request.downloadHandler.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        }

        private async Task<SheetData[]> ParseLinesToSheetData(string[] lines)
        {
            List<Task<SheetData>> tasks = new List<Task<SheetData>>();

            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                if (values.Length >= 2)
                {
                    string imageUrl = values[0].Trim();
                    string linkUrl = values[1].Trim();
                    tasks.Add(LoadImageAsync(imageUrl, linkUrl));
                }
            }

            return await Task.WhenAll(tasks);
        }

        private async Task<SheetData> LoadImageAsync(string imageUrl, string url)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Ошибка загрузки изображения: " + request.error);
                return new SheetData { Image = null, Url = url };
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return new SheetData { Image = sprite, Url = url };
        }
    }
}