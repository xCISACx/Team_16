using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MatchSettingsPopup;
    [SerializeField] private GameObject SettingsPopup;

    private bool showMatchSettings;
    
    public string GameScene;

    [SerializeField] TMP_Text _highScoreText;
    [SerializeField] TMP_Text _maxSpeedText;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        //MatchSettingsPopup.SetActive(false);
        GameManager.Instance.GameOver = false;
        GameManager.Instance.GameStarted = false;
        _maxSpeedText.text = GameManager.Instance.MaxSpeedMultiplier.ToString();
        GameManager.Instance.ScoreManager.LoadScores();
        _highScoreText.text = GameManager.Instance.Prefs.Scores[0].ScoreValue.ToString();
        GameManager.Instance.musician.PlayMusic();
    }

    public void Play()
    {
        showMatchSettings = true;
        //MatchSettingsPopup.SetActive(true);
    }
    
    public void CloseMatchSettings() //TODO: MAKE TOGGLE?
    {
        showMatchSettings = false;
        //MatchSettingsPopup.SetActive(false);
    }

    public void Options()
    {
        GameManager.Instance.MenuManager.Options();
        //SettingsPopup.SetActive(true);
    }
    
    public void CloseOptions() //TODO: MAKE TOGGLE?
    {
        GameManager.Instance.MenuManager.CloseOptions();
        //SettingsPopup.SetActive(false);
    }
    
    public void ScoreBoard()
    {
        GameManager.Instance.MenuManager.ScoreBoard();
        GameManager.Instance.ScoreManager.LoadScores();
        //SettingsPopup.SetActive(true);
    }
    
    public void CloseScoreBoard() //TODO: MAKE TOGGLE?
    {
        GameManager.Instance.MenuManager.CloseScoreBoard();
        //SettingsPopup.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
        GameManager.Instance.GameStarted = true;
        //GameManager.Instance.Init();
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
