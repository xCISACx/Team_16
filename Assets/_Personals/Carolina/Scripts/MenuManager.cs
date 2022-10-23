using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPopup;
    private bool showSettings;
    public AudioClip ButtonHoverSFX;
    public AudioClip ButtonClickSFX;
    public FullScreenMode FullScreenMode;
    [SerializeField] private Prefs prefs;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Text HighScoreText;
    [SerializeField] private GameObject Scoreboard;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Options()
    {
        if (SettingsPopup)
        {
            SettingsPopup.gameObject.SetActive(true);
        }
        
        LoadMenuUIValues();
    }
    
    public void CloseOptions() //TODO: MAKE TOGGLE?
    {
        if (SettingsPopup)
        {
            SettingsPopup.gameObject.SetActive(false);
        }
    }

    public void LoadMenuUIValues()
    {
        masterSlider.value = prefs.MasterValue;
        
        musicSlider.value = prefs.MusicValue;
        
        sfxSlider.value = prefs.SfxValue;
        
        resolutionDropdown.value = prefs.ResolutionIndex;
        
        fullscreenToggle.isOn = prefs.Fullscreen;
    }

    public void PlayButtonHoverSound()
    {
        var audioSource = GameManager.Instance.SFXSource;
        
        audioSource.PlayOneShot(ButtonHoverSFX);
    }
    
    public void PlayButtonClickSound()
    {
        var audioSource = GameManager.Instance.SFXSource;
        
        audioSource.PlayOneShot(ButtonClickSFX);
    }

    public void SetMasterVolume(float value)
    {
        var newValue = value;
        
        var newVolume = Mathf.Log10(value) * 20f;

        // for some reason Unity runs this method when the scene updates and GameManager's instance is not set by then so this is a hacky fix
        if (GameManager.Instance)
        {
            GameManager.Instance.AudioMixer.SetFloat("masterVolume", newVolume);
        }
        
        // for some reason Unity sets prefs to null after Awake so we need to get prefs from the Resources folder
        prefs = Resources.Load<Prefs>("GamePrefs");
        
        prefs.MasterValue = newValue;
        
        prefs.MasterVolume = newVolume;
    }
    
    public void SetMusicVolume(float value)
    {
        var newValue = value;
        
        var newVolume = Mathf.Log10(value) * 20f;
        
        GameManager.Instance.AudioMixer.SetFloat("musicVolume", newVolume);
        
        // for some reason Unity sets prefs to null after Awake so we need to get prefs from the Resources folder
        prefs = Resources.Load<Prefs>("GamePrefs");
        
        prefs.MusicValue = newValue;
        
        prefs.MusicVolume = newVolume;
    }
    
    public void SetSFXVolume(float value)
    {
        var newValue = value;
        
        var newVolume = Mathf.Log10(value) * 20f;
        
        GameManager.Instance.AudioMixer.SetFloat("sfxVolume", newVolume);
        
        // for some reason Unity sets prefs to null after Awake so we need to get prefs from the Resources folder
        prefs = Resources.Load<Prefs>("GamePrefs");
        
        prefs.SfxValue = newValue;
        
        prefs.SfxVolume = newVolume;
    }

    public void ToggleFullscreen(bool value)
    {
        if (value)
        {
            Screen.fullScreen = true;
            
            prefs.Fullscreen = true;
            
            FullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreen = false;
            
            prefs.Fullscreen = false;
            
            FullScreenMode = FullScreenMode.Windowed;
        }

        Screen.fullScreenMode = FullScreenMode;
        
        prefs.FullScreenMode = FullScreenMode;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, FullScreenMode);
                
                prefs.ResolutionW = 1920;
                prefs.ResolutionH = 1080;
                break;
            case 1:
                Screen.SetResolution(1366, 768, FullScreenMode);
                
                prefs.ResolutionW = 1366;
                prefs.ResolutionH = 768;
                break;
            case 2:
                Screen.SetResolution(1280, 720, FullScreenMode);
                
                prefs.ResolutionW = 1280;
                prefs.ResolutionH = 720;
                break;
            case 3:
                Screen.SetResolution(1024, 768, FullScreenMode);
                
                prefs.ResolutionW = 1024;
                prefs.ResolutionH = 768;
                break;
        }
        
        prefs.ResolutionIndex = index;
    }
    
    public void QuitToMenu()
    {
        GameManager.Instance.ResetGame();
        
        Time.timeScale = 1;

        GameManager.Instance.CurrentTimeScale = 1;
        
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void ScoreBoard()
    {
        Scoreboard.gameObject.SetActive(true);
        
        GameManager.Instance.ScoreManager.LoadScores();
    }
    
    public void CloseScoreBoard() //TODO: MAKE TOGGLE?
    {
        Scoreboard.gameObject.SetActive(false);
    }
}
