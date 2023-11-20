using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfricaLevel : MonoBehaviour
{
    public int lives;
    private int scorePoints;
    private int highScore;
    private string highScoreName;
    [SerializeField] private List<GameObject> iconsSpawnPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> correctClickableIcons = new List<GameObject>();
    [SerializeField] private List<GameObject> wrongClickableIcons = new List<GameObject>();
    public List<GameObject> stuffInBox = new List<GameObject>();
    [SerializeField] private Animator itemsBackgroundL;
    [SerializeField] private Animator itemsBackgroundR;
    private int boxesFull;
    private bool boxFull;
    [SerializeField] private Animator pauseMenu;
    [SerializeField] private Animator succesfullImage;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text highScoreNameText;
    [SerializeField] private Animator redScreenOfDeath;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip[] clips;
    private bool beginingOfLevel = true;
    [HideInInspector] public bool noMoreClicks;
    private int firstPlay;
    [HideInInspector] public bool tutorial;
    [SerializeField] private Animator tutorialHand01, tutorialHand02, tutorialHand03, tutorialHand04;
    private GameObject tutorialItemOnePlacement, tutorialItemTwoPlacement, tutorialItemThreePlacement, tutorialItemFourPlacement;
    [SerializeField] private GameObject itemOneHand01, itemOneHand02;
    [SerializeField] private GameObject itemTwoHand01, itemTwoHand02;
    [SerializeField] private GameObject itemThreeHand01, itemThreeHand02;
    [SerializeField] private GameObject itemFourHand01, itemFourHand02;
    [SerializeField] private float tutorialHandSpeed;
    [HideInInspector] public Coroutine itemOneTutorial, itemTwoTutorial, itemThreeTutorial, itemFourTutorial;

    public static AfricaLevel Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        firstPlay = PlayerPrefs.GetInt("AfricaFirstPlay");
        highScore = PlayerPrefs.GetInt("AfricaHighScore");
        highScoreName = PlayerPrefs.GetString("AfricaHighScoreName");
        highScoreText.text = highScore.ToString();
        highScoreNameText.text = highScoreName;
    }
    public void Start()
    {
        if (firstPlay == 1)
        {
            StartGame();
        }
        else
        {
            Tutorial();
        }
       
    }
    public void StartGame()
    {
        ShuffleStuffInBox();
        ShuffleItemsOnScreen();
        if (beginingOfLevel)
        {
            if (PlayerPrefs.GetInt("Music") == 0)
                musicAudioSource.mute = true;
            if (PlayerPrefs.GetInt("Sounds") == 0)
                audioSource.mute = true;
            AfricaTimer.Instance.StartTimer();
            SendBoxAudio();
            beginingOfLevel = false;
        }

    }
    private void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }
    private void ShuffleStuffInBox()
    {
        Shuffle(stuffInBox);
        foreach (GameObject item in stuffInBox)
        {
            item.SetActive(false);
        }
        stuffInBox[1].SetActive(true);
        stuffInBox[3].SetActive(true);
        stuffInBox[5].SetActive(true);
        stuffInBox[7].SetActive(true);
    }
    private void ShuffleItemsOnScreen()
    {
        Shuffle(iconsSpawnPoints);
        for (int i = 0; i < correctClickableIcons.Count; i++)
        {
            correctClickableIcons[i].gameObject.transform.position = iconsSpawnPoints[i].gameObject.transform.position;
            correctClickableIcons[i].SetActive(false);
        }
        foreach (GameObject icon in wrongClickableIcons)
        {
            icon.SetActive(false);
        }
        Shuffle(wrongClickableIcons);
        Invoke("AddWrongItemsOnScreen", 0f);
    }
    private void AddWrongItemsOnScreen()
    {

        for (int i = 0; i <= 3; i++)
        {
            wrongClickableIcons[i].gameObject.transform.position = iconsSpawnPoints[i + 8].gameObject.transform.position;
        }

    }
    public void ShowItemsBackGround()
    {
        itemsBackgroundL.SetBool("isOpen", true);
        itemsBackgroundR.SetBool("isOpen", true);
    }
    public void HideItemsBackGround()
    {
        itemsBackgroundL.SetBool("isOpen", false);
        itemsBackgroundR.SetBool("isOpen", false);
    }
    public void ShowIconsOnScreen()
    {
        foreach (GameObject icon in correctClickableIcons)
        {
            icon.SetActive(true);
        }
        for (int i = 0; i <= 3; i++)
        {
            wrongClickableIcons[i].SetActive(true);
        }
    }
    public void ReduceLives()
    {
        lives--;
        Vjeverica.Instance.SetCharacterState("wrong");
        LifeLostAudio();
        redScreenOfDeath.SetTrigger("reduceLife");
        if (lives == 2)
        {
            Heart03.Instance.SetCharacterState("srce_umire");
        }
        else if (lives == 1)
        {
            Heart02.Instance.SetCharacterState("srce_umire");
        }
        else if (lives == 0)
        {
            Heart01.Instance.SetCharacterState("srce_umire");
            CheckIfQuotaOk();
        }
    }
    private void CheckIfQuotaOk()
    {
        if (boxesFull >= 6)
        {
            noMoreClicks = true;
            PlayerPrefs.SetInt("AfricaScore", scorePoints);
            StartCoroutine(SceneTransition("africa_good"));
        }
        else
        {
            noMoreClicks = true;
            PlayerPrefs.SetInt("AfricaScore", scorePoints);
            StartCoroutine(SceneTransition("africa_bad"));
        }
    }
    IEnumerator SceneTransition(string sceneName)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }
    public void AddScorePoints()
    {
        scorePoints += 10;
        scoreText.text = scorePoints.ToString();
    }
    public void CheckIfOkToTransitionToNextBox()
    {
        boxFull = true;
        foreach (GameObject item in stuffInBox)
        {
            if(!item.activeSelf)
            {
                boxFull = false;
            }
        }
        if (boxFull)
        {
            if (tutorial)
            {
                firstPlay = 1;
                PlayerPrefs.SetInt("AfricaFirstPlay", 1);
                tutorial = false;
            }
            AfricaTimer.Instance.timerGo = false;
            HideItemsBackGround();
            Invoke("StartGame", 0.1f);
            Invoke("TimeToPack", 0.8f);
            if(boxesFull == 6)
            {
                StartCoroutine(AnimateSuccesfullImage());
                WinAudio();
            }
        }
    }
    private void TimeToPack()
    {
        Vjeverica.Instance.SetCharacterState("close_go");
        Vjeverica.Instance.CloseAllAttachments();
        //AddScorePoints();
        StopMusic();
        SendBoxAudio();
        boxesFull++;
        AfricaTimer.Instance.Accelerate();
        AfricaTimer.Instance.StartTimer();
    }
    IEnumerator AnimateSuccesfullImage()
    {
        succesfullImage.SetBool("isOpen", true);
        yield return new WaitForSeconds(1);
        succesfullImage.SetBool("isOpen", false);
    }
    public void ScorePointAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "collectPoint")
                audioSource.PlayOneShot(clip);
        }
    }
    private void LifeLostAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "lifeLost")
                audioSource.PlayOneShot(clip);
        }
    }
    private void WinAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "win")
                audioSource.PlayOneShot(clip);
        }
    }
    private void SendBoxAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "sendBox")
                audioSource.PlayOneShot(clip);
        }
    }
    public void PlayMusic()
    {
        musicAudioSource.pitch = AfricaTimer.Instance.musicPitch;
        musicAudioSource.Play();
    }
    private void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PauseGame()
    {
        StartCoroutine(PausingGame());
    }
    IEnumerator PausingGame()
    {
        pauseMenu.SetBool("isOpen", true);
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        pauseMenu.SetBool("isOpen", false);
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("africa_intro");
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("menu_games");
    }
    private void Tutorial()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
            musicAudioSource.mute = true;
        if (PlayerPrefs.GetInt("Sounds") == 0)
            audioSource.mute = true;
        tutorial = true;
        beginingOfLevel = false;
        ShuffleStuffInBox();
        ShuffleItemsOnScreen();
        SendBoxAudio();
    }
    public void TutorialHandsPlacement()
    {
        foreach( GameObject correctIcon in correctClickableIcons)
        {
            if(correctIcon.tag == stuffInBox[0].gameObject.tag)
            {
                tutorialItemOnePlacement = correctIcon;
            }
            if (correctIcon.tag == stuffInBox[2].gameObject.tag)
            {
                tutorialItemTwoPlacement = correctIcon;
            }
            if (correctIcon.tag == stuffInBox[4].gameObject.tag)
            {
                tutorialItemThreePlacement = correctIcon;
            }
            if (correctIcon.tag == stuffInBox[6].gameObject.tag)
            {
                tutorialItemFourPlacement = correctIcon;
            }
        }
        itemOneHand02.transform.position = new Vector3(tutorialItemOnePlacement.transform.position.x - 1.5f, tutorialItemOnePlacement.transform.position.y - 1f, tutorialItemOnePlacement.transform.position.z);
        itemOneHand01.transform.position = new Vector3(itemOneHand02.transform.position.x - 1.5f, itemOneHand02.transform.position.y - 1f, itemOneHand02.transform.position.z);
        itemTwoHand02.transform.position = new Vector3(tutorialItemTwoPlacement.transform.position.x - 1.5f, tutorialItemTwoPlacement.transform.position.y - 1f, tutorialItemTwoPlacement.transform.position.z);
        itemTwoHand01.transform.position = new Vector3(itemTwoHand02.transform.position.x - 1.5f, itemTwoHand02.transform.position.y - 1f, itemTwoHand02.transform.position.z);
        itemThreeHand02.transform.position = new Vector3(tutorialItemThreePlacement.transform.position.x - 1.5f, tutorialItemThreePlacement.transform.position.y - 1f, tutorialItemThreePlacement.transform.position.z);
        itemThreeHand01.transform.position = new Vector3(itemThreeHand02.transform.position.x - 1.5f, itemThreeHand02.transform.position.y - 1f, itemThreeHand02.transform.position.z);
        itemFourHand02.transform.position = new Vector3(tutorialItemFourPlacement.transform.position.x - 1.5f, tutorialItemFourPlacement.transform.position.y - 1f, tutorialItemFourPlacement.transform.position.z);
        itemFourHand01.transform.position = new Vector3(itemFourHand02.transform.position.x - 1.5f, itemFourHand02.transform.position.y - 1f, itemFourHand02.transform.position.z);
        ShowTutorialHands();
    }
    private void ShowTutorialHands()
    {
        itemOneTutorial = StartCoroutine(ItemOneTutorial());
        itemTwoTutorial = StartCoroutine(ItemTwoTutorial());
        itemThreeTutorial = StartCoroutine(ItemThreeTutorial());
        itemFourTutorial = StartCoroutine(ItemFourTutorial());
    }
    private IEnumerator ItemOneTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand01.gameObject.transform.position = itemOneHand01.transform.position;
        tutorialHand01.SetBool("isOpen", true);
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand01.gameObject.transform.position != itemOneHand02.transform.position)
            {
                tutorialHand01.gameObject.transform.position = Vector2.MoveTowards(tutorialHand01.gameObject.transform.position, itemOneHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand01.gameObject.transform.position = itemOneHand01.transform.position;

        }
    }
    private IEnumerator ItemTwoTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand02.gameObject.transform.position = itemTwoHand01.transform.position;
        tutorialHand02.SetBool("isOpen", true);
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand02.gameObject.transform.position != itemTwoHand02.transform.position)
            {
                tutorialHand02.gameObject.transform.position = Vector2.MoveTowards(tutorialHand02.gameObject.transform.position, itemTwoHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand02.gameObject.transform.position = itemTwoHand01.transform.position;

        }
    }
    private IEnumerator ItemThreeTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand03.gameObject.transform.position = itemThreeHand01.transform.position;
        tutorialHand03.SetBool("isOpen", true);
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand03.gameObject.transform.position != itemThreeHand02.transform.position)
            {
                tutorialHand03.gameObject.transform.position = Vector2.MoveTowards(tutorialHand03.gameObject.transform.position, itemThreeHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand03.gameObject.transform.position = itemThreeHand01.transform.position;

        }
    }
    private IEnumerator ItemFourTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand04.gameObject.transform.position = itemFourHand01.transform.position;
        tutorialHand04.SetBool("isOpen", true);
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand04.gameObject.transform.position != itemFourHand02.transform.position)
            {
                tutorialHand04.gameObject.transform.position = Vector2.MoveTowards(tutorialHand04.gameObject.transform.position, itemFourHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand04.gameObject.transform.position = itemFourHand01.transform.position;

        }
    }
    public void CloseItemTutorialCoroutine(int index)
    {
        if (index == 0)
        {
            StopCoroutine(itemOneTutorial);
            tutorialHand01.SetBool("isOpen", false);
        }
        if (index == 2)
        {
            StopCoroutine(itemTwoTutorial);
            tutorialHand02.SetBool("isOpen", false);
        }
        if (index == 4)
        {
            StopCoroutine(itemThreeTutorial);
            tutorialHand03.SetBool("isOpen", false);
        }
        if (index == 6)
        {
            StopCoroutine(itemFourTutorial);
            tutorialHand04.SetBool("isOpen", false);
        }
    }
}
