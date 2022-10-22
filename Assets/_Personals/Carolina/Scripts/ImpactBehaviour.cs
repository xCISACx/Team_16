using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBehaviour : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}
