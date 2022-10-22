using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<Score> Scores;
    public ScoreUI ScoreUI;

    private void Awake()
    {
        LoadScores();
    }

    public void LoadScores()
    {
        Scores = GameManager.Instance.Prefs.Scores;
        
        Scores = Scores.OrderByDescending(x => x.ScoreValue).ToList();
        
        if (ScoreUI)
        {
            ScoreUI.Populate();   
        }
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public IEnumerable<Score> GetHighScores()
    {
        return Scores.OrderByDescending(x => x.ScoreValue);
    }

    public void AddScore(Score score, bool insert, int index)
    {
        if (insert)
        {
            Scores.Insert(index, score);   
        }
        else
        {
            Scores.Add(score);
        }
    }

    public void SaveScore()
    {
        GameManager.Instance.Prefs.Scores = Scores;
    }
}
