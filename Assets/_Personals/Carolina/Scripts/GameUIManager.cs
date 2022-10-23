using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] public Canvas restartMenuCanvas;
    public GameObject RecordParent;

    // Buttons
    
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button exitGameButton1;
    [SerializeField] private Button exitGameButton2;
    
    //text
	
    [SerializeField] public TextMeshProUGUI currentWaterLevelPercentageText;
    [SerializeField] public TextMeshProUGUI currentScore;
    [SerializeField] public TextMeshProUGUI CurrentSpeed;
    [SerializeField] public TextMeshProUGUI gameOverScoreText;
    
    // Images
    [SerializeField] public Image waterFillImage;
    [SerializeField] public Image WaterLevelImage;
    [SerializeField] private Image bolt1Image;
    [SerializeField] private Image bolt2Image;
    [SerializeField] private Image bolt3Image;
    [SerializeField] private Image powerButtonImage;
    
    // Variables
    public float timerdown;
    public float currentWaterLevelPercentage = 0;
    public int maxWaterLevelPercentage = 100;
    public float currentHighScoreF;

    public bool CanSaveHighScore = true;

    [SerializeField] private string _mainMenuScene;

    public string InputName;

    private void Awake()
    {
        restartGameButton.onClick.AddListener(RestartGameButtonPressed);
        
        exitGameButton1.onClick.AddListener(ExitGameButtonPressed);
    }

    private void OnDisable()
    {
        restartGameButton.onClick.RemoveListener(RestartGameButtonPressed);
        
        exitGameButton1.onClick.RemoveListener(ExitGameButtonPressed);
    }

    public void SaveHighScore()
    {
        if (!CanSaveHighScore) return;
        
        var newScore = new Score(InputName, GameManager.Instance.Score);

        if (GameManager.Instance.ScoreManager.Scores.Count < 1)
        {
            GameManager.Instance.ScoreManager.AddScore(newScore, false, 0);
            
            GameManager.Instance.ScoreManager.SaveScore();
        }
        
        else if (GameManager.Instance.ScoreManager.Scores.Count >= 1)
        {
            var lowerScores = GameManager.Instance.ScoreManager.Scores.Where(x => x.ScoreValue <= GameManager.Instance.Score).ToList();
            
            //Debug.Log(lowerScores);

            if (lowerScores.Count > 0)
            {
                //var highestLowerScoreIndex = lowerScores.IndexOf(lowerScores.Max());

                GameManager.Instance.ScoreManager.AddScore(newScore, false, 0);

                GameManager.Instance.ScoreManager.Scores = GameManager.Instance.ScoreManager.Scores.OrderByDescending(x => x.ScoreValue).ToList();
                
                GameManager.Instance.ScoreManager.SaveScore();
            }
        }

        if (GameManager.Instance.ScoreManager.Scores.Count > 10)
        {
            Debug.Log("scores at max, removing lowest one");
            
            GameManager.Instance.ScoreManager.Scores = GameManager.Instance.ScoreManager.Scores.OrderByDescending(x => x.ScoreValue).ToList();
            
            GameManager.Instance.ScoreManager.Scores.RemoveAt(GameManager.Instance.ScoreManager.Scores.Count - 1);
            
            GameManager.Instance.ScoreManager.SaveScore();
        }

        CanSaveHighScore = false;
    }

    public void CheckIfShouldShowNameInputField()
    {
        if (GameManager.Instance.ScoreManager.Scores.Count > 0)
        {
            for (int i = 0; i < GameManager.Instance.ScoreManager.Scores.Count; i++)
            {
                var lowerScores = GameManager.Instance.ScoreManager.Scores.Where(x => x.ScoreValue <= GameManager.Instance.Score).ToList(); 
                
                if (lowerScores.Count > 0)
                {
                    RecordParent.SetActive(true);
                }
                else
                {
                    RecordParent.SetActive(false);
                }
            }   
        }
        else
        {
            RecordParent.SetActive(true);
        }
    }

    public void GetInputName(TMP_InputField inputField)
    {
        InputName = inputField.text;
    }
    
    void RestartGameButtonPressed()
    {
        //Debug.Log("init " + GameManager.Instance.Initialised);
        
        GameManager.Instance.ResetGame();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    void ExitGameButtonPressed()
    {
        Application.Quit();
    }
}
