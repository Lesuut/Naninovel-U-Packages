using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    public class LeaderBoardCoroutines : MonoBehaviour
    {
        [SerializeField] private List<string> leaderboardNames = new List<string> { "steam_id" };

        private static LeaderBoardCoroutines _instance;
        public static LeaderBoardCoroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LeaderBoardCoroutines>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("LeaderBoardCoroutines");
                        _instance = obj.AddComponent<LeaderBoardCoroutines>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, SteamLeaderboard_t> _leaderboards = new Dictionary<string, SteamLeaderboard_t>();
        private Dictionary<string, CallResult<LeaderboardFindResult_t>> _findResults = new Dictionary<string, CallResult<LeaderboardFindResult_t>>();

        public static void Initialize()
        {
            Instance.TrySetFirstGameStart();
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (!SteamAPI.IsSteamRunning())
            {
                Debug.LogError("Steam API is not running.");
                return;
            }

            // Инициализация лидербордов на старте игры
            StartCoroutine(InitLeaderboards());
        }

        private IEnumerator InitLeaderboards()
        {
            foreach (var leaderboardName in leaderboardNames)
            {
                if (!_leaderboards.ContainsKey(leaderboardName))
                {
                    // Асинхронный поиск лидерборда
                    SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(leaderboardName);
                    var findResult = new CallResult<LeaderboardFindResult_t>();
                    findResult.Set(hSteamAPICall, (pCallback, failure) => OnLeaderboardFindResult(pCallback, failure, leaderboardName));
                    _findResults[leaderboardName] = findResult;

                    // Даем время для выполнения поиска
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure, string leaderboardName)
        {
            if (!failure && pCallback.m_bLeaderboardFound != 0)
            {
                _leaderboards[leaderboardName] = pCallback.m_hSteamLeaderboard;
                Debug.Log($"Leaderboard '{leaderboardName}' initialized.");
            }
            else
            {
                Debug.LogError($"Failed to find leaderboard '{leaderboardName}'.");
            }
        }

        public void UpdateScore(string leaderboardName, int score)
        {
            // Если лидерборд еще не найден и инициализирован, добавляем его в очередь на инициализацию
            if (!_leaderboards.ContainsKey(leaderboardName))
            {
                StartCoroutine(InitLeaderboardAndUpdateScore(leaderboardName, score));
            }
            else
            {
                UploadScore(leaderboardName, score);
            }
        }

        private IEnumerator InitLeaderboardAndUpdateScore(string leaderboardName, int score)
        {
            // Запускаем инициализацию лидерборда
            Debug.Log($"Initializing leaderboard '{leaderboardName}' asynchronously...");

            // Асинхронная инициализация
            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(leaderboardName);
            var findResult = new CallResult<LeaderboardFindResult_t>();
            findResult.Set(hSteamAPICall, (pCallback, failure) => OnLeaderboardFindResult(pCallback, failure, leaderboardName));

            // Ждем завершения инициализации
            yield return new WaitUntil(() => _leaderboards.ContainsKey(leaderboardName));

            // После успешной инициализации обновляем очки
            UploadScore(leaderboardName, score);
        }

        private void UploadScore(string leaderboardName, int score)
        {
            if (_leaderboards.ContainsKey(leaderboardName))
            {
                Debug.Log($"Uploading score {score} to leaderboard '{leaderboardName}'.");

                // Загружаем очки на сервер Steam
                SteamLeaderboard_t leaderboard = _leaderboards[leaderboardName];
                SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, score, null, 0);
                var uploadResult = new CallResult<LeaderboardScoreUploaded_t>();
                uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
            }
            else
            {
                Debug.LogWarning($"Leaderboard '{leaderboardName}' is not initialized yet.");
            }
        }

        private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
        {
            if (!failure)
            {
                Debug.Log($"Score upload successful! New Rank: {pCallback.m_nGlobalRankNew}, Score: {pCallback.m_nScore}");
            }
            else
            {
                Debug.LogError($"Failed to upload score: {failure}");
            }
        }

        public void TrySetFirstGameStart()
        {
            if (!PlayerPrefs.HasKey("FirstGameStart9"))
            {
                PlayerPrefs.SetInt("FirstGameStart9", 1);
                PlayerPrefs.Save();
                UpdateScore("steam_id", 1);
                Debug.Log("<b>First Game Start!</b>");
            }
        }
    }
}
