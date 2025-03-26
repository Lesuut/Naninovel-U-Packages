using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Naninovel.U.CrossPromo
{
    public struct SheetData
    {
        public Sprite Image;
        public string Url;
        public string LeaderBoardKey;
    }

    public static class GoogleSheetDataLoader
    {
        public static async Task<SheetData[]> LoadDataAsync(string googleSheetUrl, bool debug = false)
        {
            if (debug) Debug.Log("[GoogleSheetDataLoader] LoadDataAsync started");

            string[] lines = await LoadDataFromGoogleSheet(googleSheetUrl, debug);
            if (lines == null || lines.Length == 0)
            {
                if (debug) Debug.LogWarning("[GoogleSheetDataLoader] No data received from Google Sheet");
                return Array.Empty<SheetData>();
            }

            return await ParseLinesToSheetData(lines, debug);
        }

        private static async Task<string[]> LoadDataFromGoogleSheet(string googleSheetUrl, bool debug)
        {
            if (debug) Debug.Log("[GoogleSheetDataLoader] Requesting data from: " + googleSheetUrl);

            using UnityWebRequest request = UnityWebRequest.Get(googleSheetUrl);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                if (debug) Debug.Log("[GoogleSheetDataLoader] Waiting for data...");
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (debug) Debug.LogError("[GoogleSheetDataLoader] Error loading data: " + request.error);
                return null;
            }

            if (debug) Debug.Log("[GoogleSheetDataLoader] Data received successfully");
            return request.downloadHandler.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        }

        private static async Task<SheetData[]> ParseLinesToSheetData(string[] lines, bool debug)
        {
            if (debug) Debug.Log("[GoogleSheetDataLoader] Parsing lines into SheetData");

            List<Task<SheetData>> tasks = new List<Task<SheetData>>();

            foreach (string line in lines)
            {
                if (debug) Debug.Log("[GoogleSheetDataLoader] Processing line: " + line);
                string[] values = line.Split(',');
                if (values.Length >= 3)
                {
                    string imageUrl = values[0].Trim();
                    string linkUrl = values[1].Trim();
                    string leaderBoardKey = values[2].Trim();
                    tasks.Add(LoadImageAsync(imageUrl, linkUrl, leaderBoardKey, debug));
                }
                else
                {
                    if (debug) Debug.LogWarning("[GoogleSheetDataLoader] Invalid line format: " + line);
                }
            }

            return await Task.WhenAll(tasks);
        }

        private static async Task<SheetData> LoadImageAsync(string imageUrl, string url, string leaderBoardKey, bool debug)
        {
            if (debug) Debug.Log("[GoogleSheetDataLoader] Loading image from: " + imageUrl);

            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                if (debug) Debug.Log("[GoogleSheetDataLoader] Waiting for image...");
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (debug) Debug.LogError("[GoogleSheetDataLoader] Error loading image: " + request.error);
                return new SheetData { Image = null, Url = url, LeaderBoardKey = leaderBoardKey };
            }

            if (debug) Debug.Log("[GoogleSheetDataLoader] Image loaded successfully");
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return new SheetData { Image = sprite, Url = url, LeaderBoardKey = leaderBoardKey };
        }
    }
}
