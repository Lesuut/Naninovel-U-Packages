using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Naninovel.U.CrossPromo.Commands
{
    public class LeaderBoardCoroutines : MonoBehaviour
    {
        [SerializeField] private List<string> leaderboardNames = new List<string> { "steam_id" };
        private Dictionary<string, SteamLeaderboard_t> leaderboards = new Dictionary<string, SteamLeaderboard_t>();
        private bool _initialized = false;

        private CallResult<LeaderboardFindResult_t> _findResult = new CallResult<LeaderboardFindResult_t>();
        private CallResult<LeaderboardScoreUploaded_t> _uploadResult = new CallResult<LeaderboardScoreUploaded_t>();

        // Статическая переменная для синглтона
        private static LeaderBoardCoroutines _instance;

        // Свойство для доступа к синглтону
        public static LeaderBoardCoroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Если экземпляр не существует, создаем новый объект
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
            StartCoroutine(InitSteamLeaderboards());
        }

        private IEnumerator InitSteamLeaderboards()
        {
            if (!SteamAPI.Init())
            {
                Debug.LogError("Couldn't init Steam API");
                yield break;
            }

            foreach (var name in leaderboardNames)
            {
                yield return StartCoroutine(FindAndStoreLeaderboard(name));
            }

            _initialized = true;
        }

        private IEnumerator FindAndStoreLeaderboard(string name)
        {
            if (leaderboards.ContainsKey(name))
            {
                yield break; // Если уже есть в словаре, пропускаем
            }

            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(name);
            _findResult.Set(hSteamAPICall, (result, failure) => OnLeaderboardFindResult(name, result, failure));

            yield return new WaitForSeconds(0.5f); // Даем Steam время на ответ
        }

        private void OnLeaderboardFindResult(string name, LeaderboardFindResult_t pCallback, bool failure)
        {
            if (!failure && pCallback.m_bLeaderboardFound != 0)
            {
                leaderboards[name] = pCallback.m_hSteamLeaderboard;
                Debug.Log($"STEAM LEADERBOARDS: Found {name}");
            }
            else
            {
                Debug.LogError($"STEAM LEADERBOARDS: Failed to find {name}");
            }
        }

        public void EnsureLeaderboardInitialized(string leaderboardName)
        {
            if (!leaderboards.ContainsKey(leaderboardName))
            {
                StartCoroutine(FindAndStoreLeaderboard(leaderboardName));
            }
        }

        public void UpdateScore(string leaderboardName, int score)
        {
            if (!_initialized || !leaderboards.ContainsKey(leaderboardName))
            {
                Debug.LogError($"Leaderboard {leaderboardName} is not initialized yet!");
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
    }
}
