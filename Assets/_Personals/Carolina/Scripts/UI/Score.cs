using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Score
{
    public string Name;
    public int ScoreValue;

    public Score(string name, int scoreValue)
    {
        this.Name = name;
        
        this.ScoreValue = scoreValue;
    }
}
