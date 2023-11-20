using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private Animator title, startGameButton, zemlja, mjesec;
    [SerializeField] private AudioSource musicAudioSource;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying") == 1)
        {
            int checkMusic = PlayerPrefs.GetInt("Music");
            if(checkMusic != 1)
            {
                musicAudioSource.mute = true;
            }
        }
    }
    private void Start()
    {
        Invoke("ShowTitleAndStartGameButton", 1f);
    }
    private void ShowTitleAndStartGameButton()
    {
        title.SetBool("isOpen", true);
        startGameButton.SetBool("isOpen", true);
    }
    public void StartGame()
    {
        title.SetBool("isOpen", false);
        startGameButton.SetBool("isOpen", false);
        zemlja.SetTrigger("Close");
        mjesec.SetTrigger("Close");
        Invoke("LoadGame", 0.5f);
    }
    private void LoadGame()
    {
        SceneManager.LoadScene("menu_games");
    }
}
