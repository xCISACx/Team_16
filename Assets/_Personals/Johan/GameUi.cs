using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
	
	//
	[SerializeField] private Canvas gameCanvas;
	[SerializeField] private Canvas mainMenuCanvas;
	[SerializeField] private Canvas restartMenuCanvas;
	
	
	//text
	
	[SerializeField] private TextMeshProUGUI currentWaterLevelPercentage;
	[SerializeField] private TextMeshProUGUI currentHighScore;
	[SerializeField] private TextMeshProUGUI CurrentSpeed;
	
	// Images
	[SerializeField] private Image waterFillImage;
	[SerializeField] private Image WaterLevelImage;
	[SerializeField] private Image bolt1Image;
	[SerializeField] private Image bolt2Image;
	[SerializeField] private Image bolt3Image;
	[SerializeField] private Image powerButtonImage;
	
	// Buttons
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button restartGameButton;
	[SerializeField] private Button exitGameButton1;
	[SerializeField] private Button exitGameButton2;
	
	// Variables
	public float timerdown;
	public float speed;
	public float currentWaterLevelPercentageF = 0;
	public float maxWaterLevelPercentage = 100f;
	public float currentHighScoreF;
	
    // Start is called before the first frame update
    void Start()
	{
		startGameButton.onClick.AddListener(startGameButtonPressed);
		restartGameButton.onClick.AddListener(restartGameButtonPressed);
		exitGameButton1.onClick.AddListener(exitGameButtonPressed);
		exitGameButton2.onClick.AddListener(exitGameButtonPressed);
		gameCanvas.enabled = false;
		restartMenuCanvas.enabled = false;
		mainMenuCanvas.enabled = true;
		bolt1Image.enabled = false;
		bolt2Image.enabled = false;
		bolt3Image.enabled = false;
		powerButtonImage.enabled = false;
        
		InvokeRepeating("decreasetimer", 1.0f, 0.1f);
        
    }



	void decreasetimer()
	{
		if (timerdown <= 0f)
		{
			timerdown -= 0.1f;
		}

	}



    // Update is called once per frame
    void Update()
    {
        
	    //Update Health&ActionBar
	    waterFillImage.fillAmount = currentWaterLevelPercentageF / maxWaterLevelPercentage;
	    

	    currentWaterLevelPercentage.text = Mathf.RoundToInt(currentWaterLevelPercentageF).ToString() + " %";
	    currentHighScore.text = "Current Score: " + Mathf.RoundToInt(currentHighScoreF).ToString();
	    CurrentSpeed.text = Mathf.RoundToInt(speed).ToString();
        
	    if (currentWaterLevelPercentageF == maxWaterLevelPercentage)
	    {
	    	StartCoroutine(maxPowerWaitCoroutine());
	    	
	    	currentWaterLevelPercentageF = maxWaterLevelPercentage;
	    	
		    powerButtonImage.enabled = true;
	    }
	    else if (currentWaterLevelPercentageF <= maxWaterLevelPercentage)
		{
		    bolt1Image.enabled = false;
		    bolt2Image.enabled = false;
		    bolt3Image.enabled = false;
		    powerButtonImage.enabled = false;
	    }
        
    }
    
	IEnumerator maxPowerWaitCoroutine()
	{
		timerdown = 5f;
		yield return new WaitForSeconds(5);
		if (timerdown == 0.5f || timerdown ==2f || timerdown ==3.5f)
		{
			bolt1Image.enabled = true;
			bolt2Image.enabled = false;
			bolt3Image.enabled = false;
		}
		
		if (timerdown == 1f || timerdown ==2.5f || timerdown ==4.5f)
		{
			bolt1Image.enabled = false;
			bolt2Image.enabled = true;
			bolt3Image.enabled = false;
		}
		if (timerdown == 1.5f || timerdown ==3f || timerdown ==5f)
		{
			bolt3Image.enabled = true;
			bolt2Image.enabled = false;
			bolt1Image.enabled = false;
		}
		
		
		
		
	}
    
    
	 void startGameButtonPressed()
	{
		gameCanvas.enabled = true;
		restartMenuCanvas.enabled = false;
		mainMenuCanvas.enabled = false;
		//Debug.Log(1);
	}
	 void restartGameButtonPressed()
	{
		gameCanvas.enabled = true;
		restartMenuCanvas.enabled = false;
		mainMenuCanvas.enabled = false;
		//Debug.Log(2);
        
	}
	 void exitGameButtonPressed()
	{

		Application.Quit();
		//Debug.Log(3);
        
	}
    
    
}
