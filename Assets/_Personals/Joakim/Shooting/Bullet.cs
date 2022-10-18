using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    ShootManager shootMngr;
    private void Awake()
    {
        StartCoroutine(DestroyBullet());
        shootMngr = FindObjectOfType<ShootManager>();
    }
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * shootMngr.bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ShootTarget")
        {
            GameManager.Instance.musician.PlaySound(6);
            GameManager.Instance.Score += shootMngr.scoreOnHit;
            GameManager.Instance.UpdateScoreUI();
            other.GetComponent<ShootingTarget>().KillAllTweens();
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5);
        Destroy((this.gameObject));
    }
}
