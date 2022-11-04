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

        if (GameManager.Instance.ScoreManager.Scores.Count > 0)
        {
            _highScoreText.text = GameManager.Instance.ScoreManager.Scores[0].ScoreValue.ToString();
        }
        
        //Debug.Log(GameManager.Instance.musician != null);
        
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
    }
    
    public void CloseScoreBoard()
    {
        GameManager.Instance.MenuManager.CloseScoreBoard();
    }
    
    public void HowToPlay()
    {
        GameManager.Instance.MenuManager.HowToPlay();
    }
    
    public void CloseHowToPlay()
    {
        GameManager.Instance.MenuManager.CloseHowToPlay();
    }

    public void ChangeMaxSpeed(float value)
    {
        GameManager.Instance.MaxSpeedMultiplier = value;
        
        _maxSpeedText.text = value.ToString();
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
        
        GameManager.Instance.GameStarted = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void PlayButtonHoverSound()
    {
        GameManager.Instance.MenuManager.PlayButtonHoverSound();
    }
    
    public void PlayButtonClickSound()
    {
        GameManager.Instance.MenuManager.PlayButtonClickSound();
    }
}