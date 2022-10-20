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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScores()
    {
        //var json = PlayerPrefs.GetString("scores", "{}");
        //Scores = JsonUtility.FromJson<List<Score>>(json);
        Scores = GameManager.Instance.Prefs.Scores;
        Scores = Scores.OrderByDescending(x => x.ScoreValue).ToList();
        ScoreUI.Populate();
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
        //var json = JsonUtility.ToJson(Scores);
        GameManager.Instance.Prefs.Scores = Scores;
        //PlayerPrefs.SetString("scores", json);
    }
}
