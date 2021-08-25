using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    int Score;

    private void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameState>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateScore(int newscore)
    {
        Score += newscore;
    }

    public void SetScore(int newscore)
    {
        Score = newscore;
    }

    public int GetScore()
    {
        return Score;
    }
}
