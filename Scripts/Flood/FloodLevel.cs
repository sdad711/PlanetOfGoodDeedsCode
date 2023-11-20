using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloodLevel : MonoBehaviour
{
    public int lives;
    private int scorePoints;
    private int highScore;
    private string highScoreName;
    [HideInInspector] public int floodLevel;
    [SerializeField] private GameObject wallBuilder;
    public Animator wallBuilderSpeechBubble;
    [SerializeField] private GameObject wallBuilderSandBagTarget;
    [SerializeField] private GameObject[] teamsSandBags;
    [SerializeField] private GameObject[] firstWaveSandBags;
    [SerializeField] private GameObject[] secondWaveSandBags;
    [SerializeField] private GameObject[] thirdWaveSandBags;
    [HideInInspector] public GameObject currentSandBag;
    [HideInInspector] public GameObject previousSandBag;
    [SerializeField] private Animator water;
    [SerializeField] private Animator tutorialHand;
    private bool firstWaveBuilt;
    private bool secondWaveBuilt;
    [HideInInspector] public bool thirdWaveBuilt;
    private int sandBagIndex;
    private bool wallBuilderCanAskForSandBag;
    private int firstPlay;
    [HideInInspector] public bool tutorial, showelTutorial, bagTutorial, ropeTutorial, sandBagTutorial;
    [SerializeField] private GameObject showelHand01, showelHand02, showelHand03;
    [SerializeField] private GameObject bagHand01, bagHand02, bagHand03;
    [SerializeField] private GameObject ropeHand01, ropeHand02, ropeHand03;
    [SerializeField] private GameObject sandBagHand01, sandBagHand02, sandBagHand03;
    [SerializeField] private float tutorialHandSpeed, tutorialHandSpeedTwo;
    [HideInInspector] public Coroutine tutorialCoroutine;
    [SerializeField] private Animator pauseMenu;
    [SerializeField] private Animator wallBuiltImage;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text highScoreNameText;
    private bool wallBuiltOneShot;
    [SerializeField] private Animator blueScreenOfFlood, redScreenOfDeath;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip[] clips;

    public static FloodLevel Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        firstPlay = PlayerPrefs.GetInt("FloodFirstPlay");
        highScore = PlayerPrefs.GetInt("EuropeHighScore");
        highScoreName = PlayerPrefs.GetString("EuropeHighScoreName");
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
            musicAudioSource.mute = true;
        if (PlayerPrefs.GetInt("Sounds") == 0)
            audioSource.mute = true;
        PlayMusic();
        highScoreText.text = highScore.ToString();
        highScoreNameText.text = highScoreName;
        if(firstPlay == 1)
        {
            StartGameWithoutTutorial();
        }
        else
        {
            Tutorial();
        }
    }
    public void ReduceLives()
    {
        if (lives > 0)
        {
            lives--;
            LifeLostAudio();
            redScreenOfDeath.SetTrigger("reduceLife");
        }
        if (lives == 2)
        {
            Heart03.Instance.SetCharacterState("srce_umire");
        }
        else if(lives == 1)
        {
            Heart02.Instance.SetCharacterState("srce_umire");
        }
        else if(lives == 0)
        {
            Heart01.Instance.SetCharacterState("srce_umire");
            CheckSandBagsThirdWave();
        }
    }
    public void IncreaseFloodLevel()
    {
        floodLevel++;
        if (floodLevel == 1)
        {
            StopMusic();
            AlertAudio();
            blueScreenOfFlood.SetTrigger("floodIsRising");
            StartCoroutine(IncreasingFlood("firstFlood"));
            if (wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                CloseSandBag();
            }
            CheckSandBagsFirstWave();
        }
        else if (floodLevel == 2)
        {
            StopMusic();
            AlertAudio();
            blueScreenOfFlood.SetTrigger("floodIsRising");
            StartCoroutine(IncreasingFlood("secondFlood"));
            if (wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                CloseSandBag();
            }
            CheckSandBagsSecondWave();
        }
        else if (floodLevel == 3)
        {
            StopMusic();
            AlertAudio();
            blueScreenOfFlood.SetTrigger("floodIsRising");
            StartCoroutine(IncreasingFlood("thirdFlood"));
            if (wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                CloseSandBag();
            }
            CheckSandBagsThirdWave();
        }
        else if (floodLevel > 3)
        {
            StopMusic();
            BubblesAudio();
            EveryoneCheersAudio();
            if (wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                CloseSandBag();
            }
            blueScreenOfFlood.SetTrigger("floodIsRising");
        }
    }
    IEnumerator IncreasingFlood(string boolName)
    {
        yield return new WaitForSeconds(4);
        water.SetBool(boolName, true);
    }
    private void CheckSandBagsFirstWave()
    {
        if(firstWaveBuilt)
        {
            return;
        }
        else
        {
            EverybodyHalt();
            PlayerPrefs.SetInt("EuropeScore", scorePoints);
            StartCoroutine(SceneTransition("flood_bad"));
        }
    }
    private void CheckSandBagsSecondWave()
    {
        if (secondWaveBuilt)
        {
            return;
        }
        else
        {
            EverybodyHalt();
            PlayerPrefs.SetInt("EuropeScore", scorePoints);
            StartCoroutine(SceneTransition("flood_bad"));
        }
    }
    private void CheckSandBagsThirdWave()
    {
        if (thirdWaveBuilt)
        {
            if(lives == 0)
            {
                EverybodyHalt();
                PlayerPrefs.SetInt("EuropeScore", scorePoints);
                StartCoroutine(SceneTransition("flood_good"));
            }
        }
        else
        {
            EverybodyHalt();
            PlayerPrefs.SetInt("EuropeScore", scorePoints);
            StartCoroutine(SceneTransition("flood_bad"));
        }
    }
    IEnumerator SceneTransition(string sceneName)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }

    public void EverybodyHalt()
    {
        Team01.Instance.Panic();
        Team02.Instance.Panic();
        Team03.Instance.Panic();
        Team04.Instance.Panic();
        Team05.Instance.Panic();
        CloseSandBag();
        Invoke("CloseSpeechBubble", 0.1f);
        if(!WallBuilder.Instance.puttingSandBagOnTheWall)
        {
            WallBuilder.Instance.SetCharacterState("panika");
        }
    }
    public void EverybodyChill()
    {
        Team01.Instance.Chill();
        Team02.Instance.Chill();
        Team03.Instance.Chill();
        Team04.Instance.Chill();
        Team05.Instance.Chill();
        CloseSandBag();
        Invoke("CloseSpeechBubble", 0.1f);
        if (!WallBuilder.Instance.puttingSandBagOnTheWall)
        {
            WallBuilder.Instance.SetCharacterState("veselje");
        }
    }
    public void EverybodyBackToWork()
    {
        Team01.Instance.GetBackToWork();
        Team02.Instance.GetBackToWork();
        Team03.Instance.GetBackToWork();
        Team04.Instance.GetBackToWork();
        Team05.Instance.GetBackToWork();
        WallBuilder.Instance.SetCharacterState("stoji");
        CheckIfTeamsSandBagsAreReady();
    }

    public void AddScorePoints()
    {
        scorePoints += 10;
        scoreText.text = scorePoints.ToString();
    }
    public void MoveBuilderToBag()
    {
        CloseSandBag();
        Invoke("CloseSpeechBubble", 0.1f);
        if (!firstWaveBuilt)
        {
            if(currentSandBag != null)
                previousSandBag = currentSandBag;
            currentSandBag = firstWaveSandBags[sandBagIndex].gameObject;
            Invoke("StartWalking", 0.2f);
            CloseSandBag();
        }
        else if (!secondWaveBuilt)
        {
            previousSandBag = currentSandBag;
            currentSandBag = secondWaveSandBags[sandBagIndex].gameObject;
            Invoke("StartWalking", 0.2f);
            CloseSandBag();
        }
        else if (!thirdWaveBuilt)
        {
            previousSandBag = currentSandBag;
            currentSandBag = thirdWaveSandBags[sandBagIndex].gameObject;
            Invoke("StartWalking", 0.2f);
            CloseSandBag();
        }
        else
        {
            WallBuilder.Instance.SetCharacterState("vreca_winBaci");
        }
    }
    private void StartWalking()
    {
        WallBuilder.Instance.SetCharacterState("hoda");
        StartCoroutine(MovingBuilderToBag());
    }
    IEnumerator MovingBuilderToBag()
    {
        if(previousSandBag != null)
            if (previousSandBag.activeSelf == false)
                previousSandBag.SetActive(true);
        while(wallBuilder.transform.position != currentSandBag.transform.position)
        {
            if (wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                CloseSandBag();
            }
            wallBuilder.transform.position = Vector2.MoveTowards(wallBuilder.transform.position, currentSandBag.transform.position, FloodTimer.Instance.wallBuilderSpeedOfWalking * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        WallBuilder.Instance.SetCharacterState("vreca_uzmestavi");
        GetReadyForNewSandBag();
    }
    private void GetReadyForNewSandBag()
    {
        sandBagIndex++;
        if (!firstWaveBuilt)
        {
            if (sandBagIndex > firstWaveSandBags.Length - 1)
            {
                firstWaveBuilt = true;
                sandBagIndex = 0;
            }
        }
        else if (!secondWaveBuilt)
        {
            if (sandBagIndex > secondWaveSandBags.Length -1 )
            {
                secondWaveBuilt = true;
                sandBagIndex = 0;
            }
        }
        else if (!thirdWaveBuilt)
        {
            if (sandBagIndex > thirdWaveSandBags.Length - 1)
            {
                thirdWaveBuilt = true;
                sandBagIndex = 0;
                if(thirdWaveBuilt && !wallBuiltOneShot)
                {
                    StartCoroutine(AnimateWallBuiltImage());
                    WinAudio();
                    wallBuiltOneShot = true;
                }
            }
        }
        
    }
    private void CloseSandBag()
    {
        wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    private void ShowSandBag()
    {
        wallBuilderSandBagTarget.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    private void ShowSpeechBubble()
    {
        wallBuilderSpeechBubble.SetBool("isOpen", true);
    }
    private void CloseSpeechBubble()
    {
        wallBuilderSpeechBubble.SetBool("isOpen", false);
    }
    public void CheckIfTeamsSandBagsAreReady()
    {
        wallBuilderCanAskForSandBag = false;
        foreach(GameObject sandBag in teamsSandBags)
        {
            SpriteRenderer sandBagSpriteRenderer = sandBag.GetComponent<SpriteRenderer>();
            if(sandBagSpriteRenderer.enabled == true)
            {
                wallBuilderCanAskForSandBag = true;
            }
        }
        if(wallBuilderCanAskForSandBag)
        {
            WallBuilder.Instance.SetCharacterState("zove");
            ShowSpeechBubble();
            Invoke("ShowSandBag", 0.25f);
        }
    }
    public void WallBuilderAsksForSandBag()
    {
        if(!wallBuilderCanAskForSandBag)
        {
            wallBuilderCanAskForSandBag = true;
            WallBuilder.Instance.SetCharacterState("zove");
            ShowSpeechBubble();
            Invoke("ShowSandBag", 0.25f);
        }
    }
    IEnumerator AnimateWallBuiltImage()
    {
        wallBuiltImage.SetBool("isOpen", true);
        yield return new WaitForSeconds(1);
        wallBuiltImage.SetBool("isOpen", false);
    }
    private void AlertAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "alert")
                audioSource.PlayOneShot(clip);
        }
    }
    private void BubblesAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "bubbles")
                audioSource.PlayOneShot(clip);
        }
    }
    private void EveryoneCheersAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "proslava")
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
    public void PlayMusic()
    {
        musicAudioSource.pitch = FloodTimer.Instance.musicPitch;
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
        SceneManager.LoadScene("flood_intro");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("menu_games");
    }
    private void StartGameWithoutTutorial()
    {
        CloseSpeechBubble();
        tutorial = false;
        Team01.Instance.NewWave();
        Team02.Instance.NewWave();
        Team03.Instance.NewWave();
        Team04.Instance.NewWave();
        Team05.Instance.NewWave();
        FloodTimer.Instance.StartTimer();
    }
    private void Tutorial()
    {
        tutorialCoroutine = StartCoroutine(ShowelTutorial());
        tutorial = true;
    }
    private IEnumerator ShowelTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = showelHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        showelTutorial = true;
        Team01.Instance.TutorialAskShowel();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != showelHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, showelHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = showelHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != showelHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, showelHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = showelHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != showelHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, showelHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = showelHand02.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != showelHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, showelHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = showelHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != showelHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, showelHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = showelHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != showelHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, showelHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = showelHand01.transform.position;
        }
    }
    public void StartBagTutorial()
    {
        Team01.Instance.TutorialShowelRequired();
        showelTutorial = false;
        StopCoroutine(tutorialCoroutine);
        tutorialHand.SetBool("isOpen", false);
        tutorialCoroutine = StartCoroutine(BagTutorial());
    }
    private IEnumerator BagTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = bagHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        bagTutorial = true;
        Team03.Instance.TutorialAskBag();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != bagHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, bagHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = bagHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != bagHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, bagHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = bagHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != bagHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, bagHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = bagHand02.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != bagHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, bagHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = bagHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != bagHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, bagHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = bagHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != bagHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, bagHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = bagHand01.transform.position;
        }
    }
    public void StartRopeTutorial()
    {
        Team03.Instance.TutorialBagRequired();
        bagTutorial = false;
        StopCoroutine(tutorialCoroutine);
        tutorialHand.SetBool("isOpen", false);
        tutorialCoroutine = StartCoroutine(RopeTutorial());
    }
    private IEnumerator RopeTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = ropeHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        ropeTutorial = true;
        Team02.Instance.TutorialAskRope();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != ropeHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, ropeHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = ropeHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != ropeHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, ropeHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = ropeHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != ropeHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, ropeHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = ropeHand02.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != ropeHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, ropeHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = ropeHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != ropeHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, ropeHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = ropeHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != ropeHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, ropeHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = ropeHand01.transform.position;
        }
    }
    public void StartSandBagTutorial()
    {
        Team02.Instance.TutorialRopeRequired();
        ropeTutorial = false;
        StopCoroutine(tutorialCoroutine);
        tutorialHand.SetBool("isOpen", false);
        tutorialCoroutine = StartCoroutine(SandBagTutorial());
    }
    private IEnumerator SandBagTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        sandBagTutorial = true;
        Team04.Instance.TutorialShowSandBag();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != sandBagHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, sandBagHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != sandBagHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, sandBagHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != sandBagHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, sandBagHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != sandBagHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, sandBagHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != sandBagHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, sandBagHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != sandBagHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, sandBagHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = sandBagHand01.transform.position;
        }
    }
    public void EndingTutorial()
    {
        Team04.Instance.TutorialSandBagRequired();
        sandBagTutorial = false;
        StopCoroutine(tutorialCoroutine);
        tutorialHand.SetBool("isOpen", false);
        PlayerPrefs.SetInt("FloodFirstPlay", 1);
        Invoke("StartGameWithoutTutorial", 2);
        Invoke("EndTutorialAnimations", 1.5f);
    }
    private void EndTutorialAnimations()
    {
        Team01.Instance.SetCharacterState("standing");
        Team01.Instance.ResetSandToEmpty();
        Team02.Instance.SetCharacterState("standing");
        Team03.Instance.SetCharacterState("standing");
        Team03.Instance.ResetSandToEmpty();
        Team04.Instance.SetCharacterState("standing");
        Team05.Instance.SetCharacterState("standing");
    }
}
