using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSoundFX : MonoBehaviour
{
    [SerializeField] private AudioSource rainAudioSource;
    [SerializeField] private AudioSource thunderAudioSource;
    [SerializeField] private float delayTime;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Sounds") == 0)
        {
            rainAudioSource.mute = true;
            thunderAudioSource.mute = true;
        }
        Invoke("PlaySoundFX", delayTime);
        thunderAudioSource.Play();
    }
    private void PlaySoundFX()
    {
        rainAudioSource.Play();
    }
}
