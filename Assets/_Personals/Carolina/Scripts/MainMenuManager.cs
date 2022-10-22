using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPopup;

    public string GameScene;

    [SerializeField] TMP_Text _highScoreText;
    [SerializeField] TMP_Text _maxSpeedText;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

        GameManager.Instance.GameOver = false;
        
        GameManager.Instance.GameStarted = false;
        
        _maxSpeedText.text = GameManager.Instance.MaxSpeedMultiplier.ToString();
        
        GameManager.Instance.ScoreManager.LoadScores();
        
        _highScoreText.text = GameManager.Instance.Prefs.Scores[0].ScoreValue.ToString();
        
        GameManager.Instance.musician.PlayMusic();
    }

    public void Options()
    {
        GameManager.Instance.MenuManager.Options();
    }
    
    public void CloseOptions()
    {
        GameManager.Instance.MenuManager.CloseOptions();
    }
    
    public void ScoreBoard()
    {
        GameManager.Instance.MenuManager.ScoreBoard();
        
        GameManager.Instance.ScoreManager.LoadScores();
    }
    
    public void CloseScoreBoard()
    {
        GameManager.Instance.MenuManager.CloseScoreBoard();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
        
        GameManager.Instance.GameStarted = true;
    }

    public void ChangeMaxSpeed(float value)
    {
        GameManager.Instance.MaxSpeedMultiplier = value;
        
        _maxSpeedText.text = value.ToString();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
