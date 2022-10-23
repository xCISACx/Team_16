using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Musician musician;

    public GroundGenerator GroundGenerator;

    public GameUIManager GameUIManager;

    public MenuManager MenuManager;
    
    public ChaseManager ChaseManager;

    public ScoreManager ScoreManager;

    public InputTest Player;
    
    public bool GameStarted;
    public bool GameOver;

    public int Score;
    
    public int Fuel = 100;
    public int MaxFuel = 100;

    public int CurrentLaneIndex;
    public Vector3[] LanePositions;

    public int FuelLossAmount = 20;
    public int FuelGainAmount = 10;
    
    [SerializeField] private int DefaultFuel = 50;
    [SerializeField] public bool Initialised;
    
    public float Speed = 4;
    public float SpeedMultiplier = 1;
    public float MaxSpeedMultiplier = 300;
    public float TimeBeforeSpeedIncrease;
    public float SpeedIncrease;

    public AudioMixer AudioMixer;
    public AudioSource SFXSource;
    public AudioSource MusicSource;

    public float CurrentTimeScale = 1;

    public Prefs Prefs;

    private void Awake()
    {
        if (!Instance)
        {         
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Prefs = Resources.Load<Prefs>("GamePrefs");

        SceneManager.sceneLoaded += OnSceneLoaded;
        
        MenuManager.LoadMenuUIValues();
        
        MenuManager.SetMasterVolume(Prefs.MasterValue);
        
        MenuManager.SetMusicVolume(Prefs.MusicValue);
        
        MenuManager.SetSFXVolume(Prefs.SfxValue);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "GameScene")
        {
            //Debug.Log("running init")
                
            Init();
        }
    }

    public void Update()
    {
        if(!musician)
        {
            musician = FindObjectOfType<Musician>();
        }

        if (GameOver)
        {
            Cursor.lockState = CursorLockMode.None;
        
            Cursor.visible = true;
        }
    }

    public void LoseFuel(int amount)
    {
        Fuel -= amount;
        
        Fuel = Math.Clamp(Fuel, 0, MaxFuel);

        if (Fuel <= 0)
        {
            GameOver = true;
            
            StartGameOver();
        }
        
        UpdateFuelUI();
    }
    
    public void LoseSpeed()
    {
        SpeedMultiplier /= 2;
        
        SpeedMultiplier = Math.Clamp(SpeedMultiplier, 1, MaxSpeedMultiplier);

        UpdateSpeedUI();
    }

    public void GainFuel(int amount)
    {
        Fuel += amount;
        
        Fuel = Math.Clamp(Fuel, 0, MaxFuel);
        
        UpdateFuelUI();
    }

    public void StartGameOver()
    {
        //Debug.Log("starting game over");

        GameOver = true;
        
        StopAllCoroutines();
        
        StopCoroutine(AddToMultiplier());

        GroundGenerator.moving = false;
        
        //Debug.Log("stopping ground generator movement game over");
        
        Player.CanStrafe = false;
        
        Player.CanJump = false;
        
        GameUIManager.restartMenuCanvas.gameObject.SetActive(true);

        GameUIManager.gameOverScoreText.text = "Your final score was: " + Score;
        
        GameUIManager.CheckIfShouldShowNameInputField();

        if (Score > Prefs.HighScore)
        {
            Prefs.HighScore = Score;
        }
    }

    public void UpdateFuelUI()
    {
        GameUIManager.currentWaterLevelPercentage = ( (float) Fuel / MaxFuel) * 100;
        
        //Debug.Log(GameUIManager.currentWaterLevelPercentage);
        
        GameUIManager.currentWaterLevelPercentageText.text = GameUIManager.currentWaterLevelPercentage.ToString();

        if (GameManager.Instance.GameStarted)
        {
            GameUIManager.waterFillImage.fillAmount = Mathf.InverseLerp(0, MaxFuel, Fuel);
        }

        if (Player)
        {
            Player.TankWater.GetComponent<MeshRenderer>().material.SetFloat("_Fill", Mathf.InverseLerp(0, MaxFuel, Fuel));   
        }
        //Debug.Log(GameUIManager.waterFillImage.fillAmount);
    }

    public void UpdateSpeedUI()
    {
        if (!GameManager.Instance.GameStarted) return;
        
        GameUIManager.CurrentSpeed.text = Mathf.RoundToInt(SpeedMultiplier).ToString();
        
        foreach (var wheel in Player.Wheels)
        {
            wheel.GetComponent<WheelRotate>().rotation.y =  - SpeedMultiplier * 25;
        }
    }
    
    public void UpdateScoreUI()
    {
        GameUIManager.currentScore.text = Mathf.RoundToInt(Score).ToString();
    }

    public void ResetGame()
    {
        GameOver = false;
        
        Score = 0;
        
        SpeedMultiplier = 1;
        
        Fuel = DefaultFuel;
        
        CurrentLaneIndex = 1;
        
        Initialised = false;
        
        GameUIManager.CanSaveHighScore = true;
    }

    public void Init()
    {
        //Debug.Log("Game Manager Init");
        
        musician = FindObjectOfType<Musician>();
        
        GameUIManager = FindObjectOfType<GameUIManager>();
        
        GroundGenerator = FindObjectOfType<GroundGenerator>();
        
        ChaseManager = FindObjectOfType<ChaseManager>();
        
        Player = FindObjectOfType<InputTest>();
        
        InitPlayerColours();
        
        UpdateFuelUI();
        
        UpdateSpeedUI();
        
        UpdateScoreUI();
        
        StopAllCoroutines();
        
        StartCoroutine(AddToMultiplier());
        
        musician.PlayMusic();
        
        Initialised = true;
        //Debug.Log("Game Manager Init Done");
    }

    public IEnumerator AddToMultiplier()
    {
        while (true)
        {
            //Debug.Log("speed up");
            
            SpeedMultiplier += SpeedIncrease * Time.timeScale;

            SpeedMultiplier = Mathf.Clamp(SpeedMultiplier, 1, MaxSpeedMultiplier);

            Score += Mathf.RoundToInt((SpeedMultiplier / 2) * Time.timeScale);

            if (Time.timeScale != 0)
            {
                LoseFuel(Mathf.RoundToInt((SpeedMultiplier / 8) * Time.timeScale));
            }

            UpdateSpeedUI();
            
            UpdateScoreUI();
            
            yield return new WaitForSeconds(TimeBeforeSpeedIncrease);
        }
    }

    public void InitPlayerColours()
    {
        var playerMeshRendererMaterials = Player.ModelMeshRenderer.materials;
        
        playerMeshRendererMaterials[2].color = Prefs.PantsColour;
        
        playerMeshRendererMaterials[3].color = Prefs.ShirtColour;
        
        playerMeshRendererMaterials[4].color = Prefs.BodyColour;
        
        playerMeshRendererMaterials[7].color = Prefs.CapColour;
        
        playerMeshRendererMaterials[6].color = Prefs.HairColour;
    }

    public void AddScore(int score)
    {
        Score += score;
        
        UpdateScoreUI();
    }
    
    public void ClearHighScores()
    {
        Debug.Log("CLEARING");
        Prefs.Scores.Clear();
        
        ScoreManager.Scores.Clear();

        for (int i = 0; i < 10; i++)
        {
            Prefs.Scores.Add(new Score("---", 0));
            
            ScoreManager.Scores.Add(new Score("---", 0));
        }
        
        ScoreManager.ScoreUI.Populate();
        
        Prefs.HighScore = 0;
    }
}
