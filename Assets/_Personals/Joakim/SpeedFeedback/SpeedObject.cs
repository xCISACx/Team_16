using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpeedObject : MonoBehaviour
{
    [SerializeField] public float speed;
    private SpeedEffect _speedEffect;

    private void Awake()
    {
        _speedEffect = FindObjectOfType<SpeedEffect>();
        speed = _speedEffect.fxObjSpeed;
    }

    private void Update()
    {
        transform.position -= transform.forward * Time.deltaTime * speed;

        if (transform.position.z < -10)
        {
            Destroy(this.gameObject);
        }
    }
}
