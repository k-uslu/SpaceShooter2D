using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGameSceneReplay()
    {
        FindObjectOfType<GameState>().SetScore(0);
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGameOverScene()
    {
        StartCoroutine(GameOver());        
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("GameOver");
    }

    public void LoadStartMenu()
    {
        FindObjectOfType<GameState>().SetScore(0);
        SceneManager.LoadScene("StartMenu");
    }

}
