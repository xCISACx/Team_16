using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootManager : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab, bulletSpawnPosObj, targetPrefab;
    [SerializeField] private Vector3 bulletSpawnPosition;
    bool canShoot = true;
    public float minFuelRequiredToShoot;
    public float shootCooldown = 1;
    public float bulletSpeed = 10;
    public int scoreOnHit = 50;
    public float timeToHitTarget = 10;
    public float targetSpawnSeconds;
    public float spawnCountDown = 0;

    private void Awake()
    {
        
    }

    private void Update()
    {
        bulletSpawnPosition = bulletSpawnPosObj.transform.position;

        if (spawnCountDown < targetSpawnSeconds + 0.2f)
        {
            spawnCountDown += Time.deltaTime;
        }
        else if(spawnCountDown >= targetSpawnSeconds)
        {
            SpawnTarget();
            spawnCountDown = 0;
        }

    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (canShoot && GameManager.Instance.Fuel >= minFuelRequiredToShoot)
        {
            Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
            GameManager.Instance.LoseFuel(5);
            StartCoroutine(ShootCooldown());
            canShoot = false;
        }
    }

    public void SpawnTarget()
    {
        Instantiate(targetPrefab, new Vector3(5,20,5), Quaternion.identity);
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
