using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Naninovel.U.CrossPromo
{
    public class LeaderBoardCoroutines : MonoBehaviour
    {
        [SerializeField] private List<string> leaderboardNames = new List<string> { "steam_id" };
        private Dictionary<string, SteamLeaderboard_t> leaderboards = new Dictionary<string, SteamLeaderboard_t>();
        private bool _initialized = false;

        private CallResult<LeaderboardFindResult_t> _findResult = new CallResult<LeaderboardFindResult_t>();
        private CallResult<LeaderboardScoreUploaded_t> _uploadResult = new CallResult<LeaderboardScoreUploaded_t>();

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
                    }
                }
                return _instance;
            }
        }

        private void Start()
        {
            InitSteamLeaderboards();
        }

        private void InitSteamLeaderboards()
        {
            if (!SteamAPI.Init())
            {
                Debug.LogError("Couldn't init Steam API");
                return;
            }

            foreach (var name in leaderboardNames)
            {
                FindAndStoreLeaderboard(name);
            }

            _initialized = true;
            TrySetFirstGameStart();
        }

        private void FindAndStoreLeaderboard(string name)
        {
            if (leaderboards.ContainsKey(name)) return;

            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(name);
            _findResult.Set(hSteamAPICall, (result, failure) =>
            {
                if (!failure && result.m_bLeaderboardFound != 0)
                {
                    leaderboards[name] = result.m_hSteamLeaderboard;
                    Debug.Log($"STEAM LEADERBOARDS: Found {name}");
                }
                else
                {
                    Debug.LogError($"STEAM LEADERBOARDS: Failed to find {name}");
                }
            });
        }

        public void UpdateScore(string leaderboardName, int score)
        {
            Debug.Log($"LeaderBoardCoroutines UpdateScore: {leaderboardName} score:{score}");

            if (!_initialized)
            {
                Debug.LogError($"Steam API is not initialized yet!");
                return;
            }

            if (!leaderboards.ContainsKey(leaderboardName))
            {
                Debug.Log($"Leaderboard {leaderboardName} not found, initializing...");
                StartCoroutine(FindAndStoreLeaderboard(leaderboardName, score));
                return;
            }

            SteamLeaderboard_t leaderboard = leaderboards[leaderboardName];
            SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(leaderboard,
                ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate,
                score, null, 0);

            _uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
        }

        private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
        {
            Debug.Log($"STEAM LEADERBOARDS: Upload success - {pCallback.m_bSuccess}, Score: {pCallback.m_nScore}, Changed: {pCallback.m_bScoreChanged}");
        }

        private IEnumerator FindAndStoreLeaderboard(string name, int? pendingScore = null)
        {
            if (leaderboards.ContainsKey(name))
            {
                yield break; // Уже найден
            }

            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(name);
            _findResult.Set(hSteamAPICall, (result, failure) =>
            {
                OnLeaderboardFindResult(name, result, failure, pendingScore);
            });

            yield return new WaitForSeconds(0.5f); // Даем Steam время
        }

        private void OnLeaderboardFindResult(string name, LeaderboardFindResult_t pCallback, bool failure, int? pendingScore = null)
        {
            if (!failure && pCallback.m_bLeaderboardFound != 0)
            {
                leaderboards[name] = pCallback.m_hSteamLeaderboard;
                Debug.Log($"STEAM LEADERBOARDS: Found {name}");

                if (pendingScore.HasValue)
                {
                    UpdateScore(name, pendingScore.Value); // Если был запрошен скор, обновляем его
                }
            }
            else
            {
                Debug.LogError($"STEAM LEADERBOARDS: Failed to find {name}");
            }
        }

        private void TrySetFirstGameStart()
        {
            if (!PlayerPrefs.HasKey("FirstGameStart"))
            {
                PlayerPrefs.SetInt("FirstGameStart", 1);
                PlayerPrefs.Save();
                UpdateScore("steam_id", 1);
                Debug.Log("<b>First Game Start!</b>");
            }
        }
    }
}
