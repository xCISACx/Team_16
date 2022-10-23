using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI RowUI;

    public ScoreManager ScoreManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Populate()
    {
        foreach (var row in GetComponentsInChildren<RowUI>())
        {
            DestroyImmediate(row.gameObject);
        }

        //var scores = GameManager.Instance.ScoreManager.ScoreData.Scores.ToList();
        var scores = GameManager.Instance.Prefs.Scores.ToList();
        
        for (int i = 0; i < scores.Count; i++)
        {
            var row = Instantiate(RowUI, transform).GetComponent<RowUI>();
            
            row.Rank.text = (i + 1).ToString();
            
            row.Name.text = scores[i].Name;
            
            row.Score.text = scores[i].ScoreValue.ToString();
        }
    }
}
