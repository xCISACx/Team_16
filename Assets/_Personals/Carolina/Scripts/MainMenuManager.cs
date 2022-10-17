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

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        //MatchSettingsPopup.SetActive(false);
        GameManager.Instance.GameOver = false;
        GameManager.Instance.GameStarted = false;
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
        SettingsPopup.SetActive(true);
    }
    
    public void CloseOptions() //TODO: MAKE TOGGLE?
    {
        SettingsPopup.SetActive(false);
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
}
