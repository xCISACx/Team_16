using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<Score> Scores;
    public List<Score> DefaultScores;
    public ScoreData ScoreData;
    public ScoreUI ScoreUI;

    private void Awake()
    {
        /*if (!File.Exists(GetSavePath()))
        {
            Debug.Log("created save file since one did not exist");

            ScoreData.Scores = DefaultScores.ToArray();
            
            Debug.Log("created save file since one did not exist 1");

            BinaryFormatter bf = new BinaryFormatter();
            
            Debug.Log("created save file since one did not exist 2");

            var file = File.Create(GetSavePath());

            Debug.Log("created save file since one did not exist 3");

            bf.Serialize(file, ScoreData);
            
            Debug.Log("created save file since one did not exist 4");

            ScoreData = (ScoreData) bf.Deserialize(file);
            
            file.Close();
            
            for (int i = 0; i < ScoreData.Scores.Length; i++)
            {
                Debug.Log(ScoreData.Scores[i].Name);
                Debug.Log(ScoreData.Scores[i].ScoreValue);
            }

            Debug.Log("SAVED DATA IN NEW FILE");
        }*/
        
        LoadScores();
    }
    
    /*[ContextMenu("Load Scores")]

    public void LoadScoresNotWorking()
    {
        Debug.Log("LOADING SCORES");
        
        //Scores = GameManager.Instance.Prefs.Scores;

        if (File.Exists(GetSavePath()))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(GetSavePath(), FileMode.Open);

            var scoreData = (ScoreData) bf.Deserialize(file);

            for (int i = 0; i < scoreData.Scores.Length; i++)
            {
                Debug.Log(scoreData.Scores[i].Name);
                Debug.Log(scoreData.Scores[i].ScoreValue);
            }
            
            Debug.Log(ScoreData.Scores.Length);
            
            Scores.Clear();

            Scores = ScoreData.Scores.ToList();

            Scores = Scores.OrderByDescending(x => x.ScoreValue).ToList();
            
            Debug.Log("SET SCORE TO SCORE DATA");
            
            file.Close();

            //GameManager.Instance.Prefs.Scores = Scores;
        }
        else
        {
            Debug.Log("save file does not exist");

            Scores = DefaultScores;
        }

        if (ScoreUI)
        {
            ScoreUI.Populate();   
        }
    }*/


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
        
        /*Scores = Scores.OrderByDescending(x => x.ScoreValue).ToList();

        if (Scores.Count >= 10)
        {
            Scores.RemoveAt(Scores.Count);
        }*/
    }

    public void SaveScore()
    {
        GameManager.Instance.Prefs.Scores = Scores;
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
    
    /*[ContextMenu("Save Score")]
    public void SaveScoreNotWorking()
    {
        ScoreData.Scores = Scores.ToArray();
        
        ScoreData.Scores = ScoreData.Scores.OrderByDescending(x => x.ScoreValue).ToArray();
        
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(GetSavePath(), FileMode.OpenOrCreate);

        bf.Serialize(file, ScoreData);
        
        Debug.Log("SAVING SCORES:");
        
        for (int i = 0; i < ScoreData.Scores.Length; i++)
        {
            Debug.Log(ScoreData.Scores[i].Name);
            Debug.Log(ScoreData.Scores[i].ScoreValue);
        }

        if (ScoreData.Scores.Length > 10)
        {
            Debug.Log("removing score in last place to keep list at 10");
            var tempList = ScoreData.Scores.ToList();
            tempList.RemoveAt(ScoreData.Scores.Length - 1);
            ScoreData.Scores = tempList.ToArray();
        }
        
        //GameManager.Instance.Prefs.Scores = ScoreData.Scores.ToList();
    }

    string GetSavePath()
    {
        return Application.persistentDataPath + "/ScoreData.ws";
    }*/
}

[Serializable]
public class ScoreData
{
    public Score[] Scores;
}
