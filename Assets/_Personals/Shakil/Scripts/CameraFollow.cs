﻿using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Place this script in the camera")]
	public Transform target;
	
	[SerializeField]private float smoothSpeed; //Higher value = slower camera
	
	[SerializeField]private Vector3 offset; //Manually set camera position in the inspector
	private Vector3 velocity = Vector3.zero;

	private void LateUpdate()
	{
		Vector3 targetPosition = target.position + offset; 
		Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
		transform.position = smoothedPosition;
	}
}
