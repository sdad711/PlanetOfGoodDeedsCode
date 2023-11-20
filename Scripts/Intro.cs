using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private AudioSource introSource;
    [SerializeField] private GameObject subtitles;
    [SerializeField] private string sceneName;
    private bool narationStart;
    [SerializeField] private float narationStartDelay;
    [SerializeField] private GameObject skipButton;
    private void Awake()
    {
        var titleAudioSource = GameObject.FindGameObjectWithTag("TitleAudioSource");
        if (titleAudioSource != null)
            Destroy(titleAudioSource.gameObject);
    }
    private void Start()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("Sounds") == 0)
            introSource.mute = true;
        if (sceneName == "africa")
        {
            var checkIfFirstTime = PlayerPrefs.GetInt("AfricaIntro");
            if(checkIfFirstTime == 1)
            {
                Invoke("ShowSkipButton", 2f);
            }
            else
            {
                PlayerPrefs.SetInt("AfricaIntro", 1);
                PlayerPrefs.SetInt("PlayerPrefsAfrica", 1);
            }
        }
        else if (sceneName == "flood")
        {
            var checkIfFirstTime = PlayerPrefs.GetInt("EuropeIntro");
            if (checkIfFirstTime == 1)
            {
                Invoke("ShowSkipButton", 2f);
            }
            else
            {
                PlayerPrefs.SetInt("EuropeIntro", 1);
                PlayerPrefs.SetInt("PlayerPrefsEurope", 1);
            }
        }
        else if (sceneName == "jungle")
        {
            var checkIfFirstTime = PlayerPrefs.GetInt("IndiaIntro");
            if (checkIfFirstTime == 1)
            {
                Invoke("ShowSkipButton", 2f);
            }
            else
            {
                PlayerPrefs.SetInt("IndiaIntro", 1);
                PlayerPrefs.SetInt("PlayerPrefsIndia", 1);
            }
        }
        Invoke("StartNaration", narationStartDelay);
    }
    private void StartNaration()
    {
        Vjeverica.Instance.SetCharacterState("avatar_govori");
        introSource.Play();
        if (PlayerPrefs.GetInt("Subtitles") == 1)
            subtitles.SetActive(true);
        narationStart = true;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    private void ShowSkipButton()
    {
        skipButton.SetActive(true);
    }
    private void Update()
    {
        if(narationStart)
            if(!introSource.isPlaying)
            {
                Vjeverica.Instance.SetCharacterState("avatar_stoji");
                Invoke("LoadScene", 1f);
                narationStart = false;
            }
    }
}

