using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] public Canvas restartMenuCanvas;
    
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

    [SerializeField] private string _mainMenuScene;

    private void Awake()
    {
        restartGameButton.onClick.AddListener(RestartGameButtonPressed);
        exitGameButton1.onClick.AddListener(ExitGameButtonPressed);
        //exitGameButton2.onClick.AddListener(ExitGameButtonPressed);
    }

    private void OnDisable()
    {
        restartGameButton.onClick.RemoveListener(RestartGameButtonPressed);
        exitGameButton1.onClick.RemoveListener(ExitGameButtonPressed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void RestartGameButtonPressed()
    {
        //gameCanvas.enabled = true;
        //restartMenuCanvas.enabled = false;
        //mainMenuCanvas.enabled = false;
        GameManager.Instance.Initialised = false;
        //Debug.Log("init " + GameManager.Instance.Initialised);
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Debug.Log(2);
        
    }
    void ExitGameButtonPressed()
    {
        //SceneManager.LoadScene(_mainMenuScene);
        Application.Quit();
        //Debug.Log(3);
        
    }
}
