using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingsUI : MonoBehaviour
{
    [SerializeField] private Animator menu;
    [SerializeField] private Animator highScoreMenu;
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject subtitles;
    [SerializeField] private string sceneName;
    private bool narationStart;
    [SerializeField] private float timeToShow;
    private bool isHighScore;
    private int score, highScore;
    private string highScoreName;
    [SerializeField] private Text newHighScoreText;
    [SerializeField] private Text scoreText;
    [SerializeField] private InputField inputField;
    private string thisSceneName;
    private void Awake()
    {
        thisSceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "africa")
        {
            score = PlayerPrefs.GetInt("AfricaScore");
            highScore = PlayerPrefs.GetInt("AfricaHighScore");
        }
        else if (sceneName == "flood")
        {
            score = PlayerPrefs.GetInt("EuropeScore");
            highScore = PlayerPrefs.GetInt("EuropeHighScore");
        }
        else if (sceneName == "jungle")
        {
            score = PlayerPrefs.GetInt("IndiaScore");
            highScore = PlayerPrefs.GetInt("IndiaHighScore");
        }
        if (thisSceneName == "africa_good")
        {
            PlayerPrefs.SetInt("AfricaFlag", 1);
        }
        else if (thisSceneName == "flood_good")
        {
            PlayerPrefs.SetInt("EuropeFlag", 1);
        }
        else if (thisSceneName == "jungle_good")
        {
            PlayerPrefs.SetInt("IndiaFlag", 1);
        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
            musicSource.mute = true;
        if (PlayerPrefs.GetInt("Sounds") == 0)
            introSource.mute = true;
        if (score > highScore)
        {
            highScore = score;
            isHighScore = true;
        }
        newHighScoreText.text = highScore.ToString();
        scoreText.text = score.ToString();
        Invoke("StartNaration", timeToShow);

    }
    private void StartNaration()
    {
        Vjeverica.Instance.SetCharacterState("avatar_govori");
        introSource.Play();
        if (PlayerPrefs.GetInt("Subtitles") == 1)
            subtitles.SetActive(true);
        narationStart = true;
    }
    private void Update()
    {
        if (narationStart)
            if (!introSource.isPlaying)
            {
                Vjeverica.Instance.SetCharacterState("avatar_stoji");
                if (isHighScore)
                    Invoke("ShowHighScoreMenu", 0);
                else
                    Invoke("ShowMenu", 0f);
                subtitles.SetActive(false);
                narationStart = false;
            }
    }
    private void ShowMenu()
    {
        menu.SetBool("isOpen", true);
    }
    private void ShowHighScoreMenu()
    {
        highScoreMenu.SetBool("isOpen", true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("menu_games");
    }
    public void SubmitName()
    {
        highScoreName = inputField.text;
        if (highScoreName == "")
        {
            var localeIndex = PlayerPrefs.GetInt("Locale");
            if (localeIndex == 0)
                highScoreName = "VOLONTER/KA";
            else if (localeIndex == 1)
                highScoreName = "VOLUNTEER";
            else if (localeIndex == 2)
                highScoreName = "ВОЛОНТЕР/КА";
        }
        if (sceneName == "africa")
        {
            PlayerPrefs.SetString("AfricaHighScoreName", highScoreName);
            PlayerPrefs.SetInt("AfricaHighScore", highScore);
        }
        else if (sceneName == "flood")
        {
            PlayerPrefs.SetString("EuropeHighScoreName", highScoreName);
            PlayerPrefs.SetInt("EuropeHighScore", highScore);
        }
        else if (sceneName == "jungle")
        {
            PlayerPrefs.SetString("IndiaHighScoreName", highScoreName);
            PlayerPrefs.SetInt("IndiaHighScore", highScore);
        }
        scoreText.text = highScore.ToString();
        highScoreMenu.SetBool("isOpen", false);
        Invoke("ShowMenu", 0.2f);
    }
  
}
