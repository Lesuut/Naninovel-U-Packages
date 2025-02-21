using UnityEngine;

public class PromoLeaderboard : MonoBehaviour
{
    /*private LeaderBoardCoroutines _leaderboard;
    private bool _wasOpened = false;

    public void OnPromoClicked(string title)
    {
        _leaderboard ??= Camera.main.transform.GetComponentInChildren<LeaderBoardCoroutines>();

        if (_leaderboard != null)
        {
            _leaderboard.UpdateScore(title, 1);

            if (!_wasOpened)
            {
                OnScreenOpen();
            }

            Debug.Log("send stats: " + title);
        }
        else
        {
            Debug.Log("no lb", Camera.main);
        }
    }

    private void OnScreenOpen()
    {
        *//*_leaderboard.UpdateScore(_screen.OpenWay, 1);
        Debug.Log("send stats: " + _screen.OpenWay);
        _wasOpened = true;*//*
    }

    public void OnScreenHide()
    {
        *//*_screen.OpenWay = "";
        _wasOpened = false;*//*
    }*/
}