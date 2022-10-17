using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Musician : MonoBehaviour
{
    public MusicPack musicPack;
    public AudioMixer masterMixer;

    private void Awake()
    {
        GameManager.Instance.MusicSource.GetComponent<AudioSource>().clip = musicPack.music;
    }
    public void PlayMusic()
    {
        GameManager.Instance.MusicSource.Play();
    }
    public void PlaySound(int index)
    {
        GameManager.Instance.SFXSource.PlayOneShot(musicPack.SFX[index]);
    }
}
