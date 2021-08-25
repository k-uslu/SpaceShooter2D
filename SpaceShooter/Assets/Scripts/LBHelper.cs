using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LBHelper : MonoBehaviour {

    [SerializeField] InputField inp;
    [SerializeField] Button submit;

    [SerializeField] CustomLeaderboard cl;

    GameObject scoreboard;
    [SerializeField] Text lbtext;
    [SerializeField] string data;

    

    public void addScore()
    {
        string name = inp.text;
        int score = FindObjectOfType<GameState>().GetScore();
        cl.AddScore(name, score);

        Destroy(inp);
        Destroy(submit);

        scoreboard.SetActive(true);
        StartCoroutine(Waiter());        
    }

    IEnumerator Waiter()
    {
        while (cl.response == 0)
        {
            yield return null;
        }
        lbtext.text = "Loading";
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        cl.GetScores();
        while (cl.highScores == "")
        {
            yield return null;
        }
        string str="";     
        for (var i = 0; i < cl.scorearray.Length; i++)
        {
            str = str + cl.scorearray[i].name.ToString()+" : "+ cl.scorearray[i].score.ToString() + "\n";
        }
        lbtext.text = str;
    }

    public void Start()
    {
        scoreboard = GameObject.Find("Leaderboard");
        scoreboard.SetActive(false);
    }

    
}

