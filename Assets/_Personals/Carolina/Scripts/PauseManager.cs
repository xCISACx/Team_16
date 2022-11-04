using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PauseManager : MonoBehaviour
{
    public bool GamePaused = false;
    
    public Canvas PauseCanvas;
    
    public MenuManager MenuManager;

    public void TogglePause()
    {
        if (!GameManager.Instance.GameOver)
        {
            GamePaused = !GamePaused;
            
            PauseCanvas.enabled = GamePaused;
            
            if (MenuManager)
            {
                MenuManager.CloseOptions();
                
            }

            if (GamePaused)
            {
                Time.timeScale = 0;
                
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = GameManager.Instance.CurrentTimeScale;

                if (MenuManager)
                {
                    MenuManager.CloseOptions();   
                }

                Cursor.lockState = CursorLockMode.Locked;
            }   
        }
    }
    
    public void Options()
    {
        GameManager.Instance.MenuManager.Options();
    }

    public void HowToPlay()
    {
        GameManager.Instance.MenuManager.HowToPlay();
    }

    public void QuitToMenu()
    {
        GameManager.Instance.MenuManager.QuitToMenu();
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
