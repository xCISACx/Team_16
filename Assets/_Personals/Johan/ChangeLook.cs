using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeLook : MonoBehaviour
{
	
	private int shirtColorInt =1;
	private int hatColorInt =1;
	
	
	[SerializeField] private Canvas changeColorCanvas;
	
	//text
	
	[SerializeField] private TextMeshProUGUI currentWaterLevelPercentage;

	// Images
	
	
	// Buttons
	[SerializeField] private Button changeShirtColor;
	[SerializeField] private Button changeHatColor;
	[SerializeField] private Button back;
	

    void Start()
    {
	    changeShirtColor.onClick.AddListener(changeShirtColorPressed);
	    changeHatColor.onClick.AddListener(changeHatColorPressed);
	    back.onClick.AddListener(backPressed);
    }

    
    void Update()
    {
        
    }
    
	void changeShirtColorPressed()
	{
		shirtColorInt = shirtColorInt + 1;
		if (shortColorInt <=5)
		{
			shirtColorInt = 1;
		}
	}
	void changeHatColorPressed()
	{
		hatColorInt = hatColorInt + 1;
		if (hatColorInt <=5)
		{
			hatColorInt = 1;
		}
	}
	void backPressed()
	{
		changeColorCanvas.enabled = false;
		//EnableMainMenuCanavas here
	}
}
