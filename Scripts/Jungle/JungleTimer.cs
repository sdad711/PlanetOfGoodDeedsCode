using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleTimer : MonoBehaviour
{
    public float askingTime;
    public float workingTime;
    public float restartTime;
    public float animationSpeed;
    public float sceneToSceneTransitionSpeed;
    public float musicPitch;
    public float speechBubbleColorSmoothness;
    [SerializeField] private float percentageOfAcceleration;
    public static JungleTimer Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void Accelerate()
    {
        sceneToSceneTransitionSpeed /= percentageOfAcceleration;
        workingTime *= percentageOfAcceleration;
        askingTime *= percentageOfAcceleration;
        restartTime *= percentageOfAcceleration;
        animationSpeed /= percentageOfAcceleration;
        musicPitch /= percentageOfAcceleration;
        speechBubbleColorSmoothness *= percentageOfAcceleration;
    }
}
