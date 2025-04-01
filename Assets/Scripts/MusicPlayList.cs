using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayList : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }
    private void Update()
    {
        if (!audioSource.isPlaying && !AudioListener.pause)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
    }
    private AudioClip GetRandomClip()
    {
        
       return clips[Random.Range(0, clips.Length)];

    }
}
