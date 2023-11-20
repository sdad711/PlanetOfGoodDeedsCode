using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfricaTimer : MonoBehaviour
{
    public float animationSpeed;
    public float musicPitch;
    [SerializeField] private float timeToFillBox;
    [SerializeField] private float percentageOfAcceleration;
    private float timer;
    [SerializeField] private Text timerText;
    public bool timerGo;
    public static AfricaTimer Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void StartTimer()
    {
        timer = timeToFillBox;
        timerGo = true;
    }
    private void Restart()
    {
        AfricaLevel.Instance.StartGame();
    }
    public void Accelerate()
    {
        timeToFillBox *= percentageOfAcceleration;
        animationSpeed /= percentageOfAcceleration;
        musicPitch /= percentageOfAcceleration;
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
                if (AfricaLevel.Instance.noMoreClicks == false)
                {
                    AfricaLevel.Instance.ReduceLives();
                    if (AfricaLevel.Instance.lives != 0)
                    {
                        AfricaLevel.Instance.HideItemsBackGround();
                        Vjeverica.Instance.CloseAllAttachments();
                        Vjeverica.Instance.SetCharacterState("close_go");
                        Accelerate();
                        Invoke("Restart", 0.1f);
                    }
                    timerGo = false;
                }

            }
        }
    }
}
