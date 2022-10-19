using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class ShootingTarget : MonoBehaviour
{
    ShootManager shootManager;
    float r, g, b;
    public Vector3 point1, point2, point3;
    private Sequence spawnSeq;
    private Sequence activeSeq;
    private Sequence despawnSeq;
    private Sequence animate;
    public TextMeshProUGUI countdownTxt;
    public float timeLeftUntilDespawn;
    private void Awake()
    {
        shootManager = FindObjectOfType<ShootManager>();
        DOTween.Init();
        timeLeftUntilDespawn = shootManager.timeToHitTarget;
        Spawn();
        SetColor();
        StartCoroutine(Despawn());
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            Destroy(gameObject);
            KillAllTweens();
        }
        
        timeLeftUntilDespawn -= Time.deltaTime;
        countdownTxt.text = Mathf.RoundToInt(timeLeftUntilDespawn).ToString();
    }

    public void KillAllTweens()
    {
        DOTween.KillAll();
    }
    
    void SetColor()
    {
        r = Random.Range(10, 255);
        g = Random.Range(10, 255);
        b = Random.Range(10, 255);
        GetComponent<Renderer>().material.color = new Color32((byte)r, (byte)g, (byte)b, 255);
    }

    void Animate()
    {
        Sequence animate = DOTween.Sequence();
       animate.Append(this.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 1, 2, 1))
           .Append(this.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1, 2, 1))
           .OnComplete(() => {
               animate.Restart();
           });
    }
    void Spawn()
    {
        spawnSeq = DOTween.Sequence();
        spawnSeq.Append(this.transform.DOMoveY(1.5f, 2, false))
            .OnComplete(() =>
            {
                InterpolateBetweenPoints();
                Animate();
                spawnSeq.Kill();
            });
    }

    void InterpolateBetweenPoints()
    {
        Animate();
        activeSeq = DOTween.Sequence();
            activeSeq.AppendCallback(() =>
            {
                GameManager.Instance.musician.PlaySound(5);
            })
            .Append(this.transform.DOMove(point1, 1, false))
            .AppendCallback(() =>
            {
                GameManager.Instance.musician.PlaySound(5);
            })
            .Append(this.transform.DOMove(point2, 1, false))
            .AppendCallback(() =>
            {
                GameManager.Instance.musician.PlaySound(5);
            })
            .Append(this.transform.DOMove(point3, 1, false))
            .AppendCallback(() =>
            {
                GameManager.Instance.musician.PlaySound(5);
            })
            .OnComplete(() => {
                activeSeq.Restart();
        });
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(shootManager.timeToHitTarget);
        activeSeq.Kill();
        animate.Kill();
        despawnSeq = DOTween.Sequence();
        despawnSeq.Append(this.transform.DOMoveY(20, 2, false))
                   .OnComplete(() => {
                       despawnSeq.Kill();                      
                        });
        yield return new WaitForSeconds(2);
        KillAllTweens();
        Destroy(this.gameObject);
    }
}
