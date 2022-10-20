using System;
using UnityEngine;

public class InvincTempScript : MonoBehaviour
{
    private ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    
    public void InvincibleOn()
    {
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = new Color(0f, 0.24f, 1f);
    }
    public void InvincibleOff()
    {
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = new Color(1f,1f,1f);
    }

}
