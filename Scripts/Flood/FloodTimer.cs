using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloodTimer : MonoBehaviour
{
    public float askingTime;
    public float workingTime;
    public float restartTime;
    public float wallBuilderSpeedOfWalking;
    public float animationSpeed;
    public float musicPitch;
    public float speechBubbleColorSmoothness;
    [SerializeField] private float floodTime;
    [SerializeField] private float percentageOfAcceleration;
    private float timer;
    [SerializeField] private Text timerText;
    private bool timerGo;
    [HideInInspector] public bool flood;
    public static FloodTimer Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void StartTimer()
    {
        timer = floodTime;
        timerGo = true;
    }
    private IEnumerator FloodRises()
    {
        timerGo = false;
        flood = true;
        timer = floodTime;
        if (FloodLevel.Instance.floodLevel < 4)
        {
            FloodLevel.Instance.EverybodyHalt();
        }
        else
        {
            FloodLevel.Instance.EverybodyChill();
        }
        yield return new WaitForSeconds(5);
        FloodLevel.Instance.PlayMusic();
        FloodLevel.Instance.EverybodyBackToWork();
        timerGo = true;
        flood = false;
    }
    private void Update()
    {
        if (timerGo)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                int minutes = (int)(timer / 60);
                int seconds = (int)(timer % 60);
                if (minutes < 10 && seconds < 10)
                    timerText.text = "0" + minutes + ":" + "0" + seconds;
                else if (minutes < 10 && seconds >= 10)
                    timerText.text = "0" + minutes + ":" + seconds;
                else if (minutes >= 10 && seconds < 10)
                    timerText.text = minutes + ":0" + seconds;
                else
                    timerText.text = minutes + ":" + seconds;
            }
            if (timer <= 0)
            { 
                FloodLevel.Instance.IncreaseFloodLevel();
                floodTime *= percentageOfAcceleration;
                workingTime *= percentageOfAcceleration;
                askingTime *= percentageOfAcceleration;
                restartTime *= percentageOfAcceleration;
                wallBuilderSpeedOfWalking /= percentageOfAcceleration;
                animationSpeed /= percentageOfAcceleration;
                musicPitch /= percentageOfAcceleration;
                speechBubbleColorSmoothness *= percentageOfAcceleration;
                StartCoroutine(FloodRises());
            }
        }
    }
}
