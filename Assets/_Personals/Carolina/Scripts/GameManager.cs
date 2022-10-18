using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Musician musician;

    public GroundGenerator GroundGenerator;

    public GameUIManager GameUIManager;

    public MenuManager MenuManager;

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

    public Image SpeedPointer;
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
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "GameScene")
        {
            Debug.Log("running init");
            Init();
        }
    }

    public void Update()
    {
        // unity is stupid and doesn't run the init function so we need to reassign one by one
        if (GameStarted)
        {
            /*if (!GroundGenerator)
            {
                GroundGenerator = FindObjectOfType<GroundGenerator>();
            }

            if (!GameUIManager)
            {
                GameUIManager = FindObjectOfType<GameUIManager>();
            }

            if (!Player)
            {
                Player = FindObjectOfType<InputTest>();
            }*/
        }
        
        if(!musician)
        {
            musician = FindObjectOfType<Musician>();
        }

        /*if (!Initialised && GameStarted)
        {
           Init();
        }*/

        if (GameOver)
        {
            Cursor.lockState = CursorLockMode.None;
        
            Cursor.visible = true;
        }
        
        /*if (!Initialised)
        {
            Player = FindObjectOfType<InputTest>();
            StopAllCoroutines();
            StartCoroutine(AddToMultiplier());
            GameUIManager = FindObjectOfType<GameUIManager>();
            GroundGenerator = FindObjectOfType<GroundGenerator>();
            Player = FindObjectOfType<InputTest>();
            UpdateFuelUI();
            UpdateSpeedUI();
            UpdateScoreUI();
        
            CurrentLaneIndex = 1;

            GameInitDone = true;
        }*/
    }

    /*public void UpdateDropPosition(Vector3 pos)
    {
        DropManager.nextDropPos = pos;
    }*/
    
    public void LoseFuel(int amount)
    {
        Fuel -= amount;
        Fuel = Math.Clamp(Fuel, 0, MaxFuel);

        if (Fuel <= 0)
        {
            GameOver = true;
            StartGameOver();
            //Player.CanJump = false;
        }
        
        UpdateFuelUI();
    }
    
    public void LoseSpeed()
    {
        SpeedMultiplier = 1;
        
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
        
        StopAllCoroutines();
        
        StopCoroutine(AddToMultiplier());

        GroundGenerator.moving = false;
        
        Player.CanStrafe = false;
        
        GameUIManager.restartMenuCanvas.gameObject.SetActive(true);

        GameUIManager.gameOverScoreText.text = "Your final score was: " + Score;

        if (Score > Prefs.HighScore)
        {
            Prefs.HighScore = Score;
        }
        
        //Time.timeScale = 0;
    }

    public void UpdateFuelUI()
    {
        GameUIManager.currentWaterLevelPercentage = ( (float) Fuel / MaxFuel) * 100;
        
        ////Debug.Log(GameUIManager.currentWaterLevelPercentage);
        
        GameUIManager.currentWaterLevelPercentageText.text = GameUIManager.currentWaterLevelPercentage.ToString();
        
        GameUIManager.waterFillImage.fillAmount = Mathf.InverseLerp(0, MaxFuel, Fuel);
        Player.TankWater.GetComponent<MeshRenderer>().material.SetFloat("_Fill", Mathf.InverseLerp(0, MaxFuel, Fuel));
        ////Debug.Log(GameUIManager.waterFillImage.fillAmount);
    }

    public void UpdateSpeedUI()
    {
        GameUIManager.CurrentSpeed.text = Mathf.RoundToInt(SpeedMultiplier).ToString();
        
        foreach (var wheel in Player.Wheels)
        {
            wheel.GetComponent<WheelRotate>().rotation.y =  - SpeedMultiplier * 25;
        }
        
        //SpeedPointer.transform.Rotate();
    }
    
    public void UpdateScoreUI()
    {
        GameUIManager.currentScore.text = Mathf.RoundToInt(Score).ToString();
        //SpeedPointer.transform.Rotate();
    }

    public void ResetGame()
    {
        GameOver = false;
        Score = 0;
        SpeedMultiplier = 1;
        Fuel = DefaultFuel;
        CurrentLaneIndex = 1;
        Initialised = false;
        //Player.Init();
        //Init();
    }

    public void Init()
    {
        //Debug.Log("Game Manager Init");
        musician = FindObjectOfType<Musician>();
        GameUIManager = FindObjectOfType<GameUIManager>();
        GroundGenerator = FindObjectOfType<GroundGenerator>();
        Player = FindObjectOfType<InputTest>();
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
            ////Debug.Log("speed up");
            
            SpeedMultiplier += SpeedIncrease * Time.timeScale;

            SpeedMultiplier = Mathf.Clamp(SpeedMultiplier, 1, MaxSpeedMultiplier);

            Score += Mathf.RoundToInt((SpeedMultiplier / 2) * Time.timeScale);

            if (Time.timeScale != 0)
            {
                LoseFuel(Mathf.RoundToInt(GameManager.Instance.SpeedMultiplier / 2));   
            }

            UpdateSpeedUI();
            
            UpdateScoreUI();
            
            yield return new WaitForSeconds(TimeBeforeSpeedIncrease);
        }
    }

    public void AddScore(int score)
    {
        Score += score;
        UpdateScoreUI();
    }
}
