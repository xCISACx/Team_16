using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
	[SerializeField] private float MAX_SPEED_ANGLE = -45;
	[SerializeField] private float ZERO_SPEED_ANGLE = 230;
	
	
	[SerializeField] private Transform needleTranform;
	
	private float speedMax;
	private float speed;

	[SerializeField] private int step;
	
	
	private void Awake()
	{
		speed = GameManager.Instance.SpeedMultiplier;
		
		speedMax = GameManager.Instance.MaxSpeedMultiplier;
	}
  
	
    void Update()
    {
	    speed = GameManager.Instance.SpeedMultiplier;

	    speed = Mathf.Clamp(speed, 0, speedMax);

	    needleTranform.eulerAngles = new Vector3(0,0, Mathf.Lerp(ZERO_SPEED_ANGLE, MAX_SPEED_ANGLE, speed / speedMax));
    }
}