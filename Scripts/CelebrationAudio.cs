using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationAudio : MonoBehaviour
{
    [SerializeField] AudioSource celebrationAudio;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Sounds") == 0)
            celebrationAudio.mute = true;
    }
}
