using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpeedEffect : MonoBehaviour
{
    [SerializeField] private float effectIntensity;
    [SerializeField] private float effectSpawnInterval;
    [SerializeField] private float count = 0;
    public float fxObjSpeed;
    [SerializeField] private float multiplier;
    public GameObject fxPrefab;

    [Range(0.0f, 10.0f)] [SerializeField] private float multiplySpeed;

    public float mySliderFloat;

    private void Update()
    {
        multiplier = GameManager.Instance.SpeedMultiplier;
        SetEffectIntrensity();

        fxObjSpeed = ((effectIntensity * 25) + 20) * multiplySpeed;

        Vector3 randomSpawnPos = new Vector3(Random.Range(-5, 5), Random.Range(1, 5.5f), 30);
        
        if (multiplier > 3)
        {
            if (count < effectSpawnInterval + 0.2f)
            {
                count += Time.deltaTime;
            }
            else if (count >= effectSpawnInterval)
            {
                randomSpawnPos = new Vector3(Random.Range(-5, 5), Random.Range(1, 5.5f), 30);
                Instantiate(fxPrefab, randomSpawnPos, Quaternion.identity);
                randomSpawnPos = new Vector3(Random.Range(-5, 5), Random.Range(1, 5.5f), 30);
                Instantiate(fxPrefab, randomSpawnPos, Quaternion.identity);
                randomSpawnPos = new Vector3(Random.Range(-5, 5), Random.Range(1, 5.5f), 30);
                Instantiate(fxPrefab, randomSpawnPos, Quaternion.identity);
                count = 0;
            }
        }
    }

    public void switchEffectIntensity()
    {
        switch (effectIntensity)
        {
            case 0: effectSpawnInterval = 2f;
                break;
            case 1: effectSpawnInterval = 1.5f;
                break;
            case 2: effectSpawnInterval = 1.2f;
                break;
            case 3: effectSpawnInterval = 0.9f;
                break;
            case 4: effectSpawnInterval = 0.5f;
                break;
            case 5: effectSpawnInterval = 0.3f;
                break;
            case 6: effectSpawnInterval = 0.1f;
                break;
        }
    }

    public void SetEffectIntrensity()
    {
        if (multiplier < 3)
        {
            return;
        }
        if (multiplier > 3 && multiplier < 6) { effectIntensity = 0; switchEffectIntensity();}
        if (multiplier > 6 && multiplier < 9) { effectIntensity = 1; switchEffectIntensity();}
        if (multiplier > 9 && multiplier < 12) { effectIntensity = 2; switchEffectIntensity();}
        if (multiplier > 12 && multiplier < 15) { effectIntensity = 3; switchEffectIntensity();}
        if (multiplier > 15 && multiplier < 18) { effectIntensity = 4; switchEffectIntensity();}
        if (multiplier > 18 && multiplier < 21) { effectIntensity = 5; switchEffectIntensity();}
        if (multiplier > 21) { effectIntensity = 6; }
        
    }
}
