using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class JungleLevel : MonoBehaviour
{
    public int lives;
    private int scorePoints;
    private int highScore;
    private string highScoreName;
    [SerializeField] private List<GameObject> levelFirstSpawnPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> levelSecondSpawnPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> levelThirdSpawnPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> firstLevelJunk = new List<GameObject>();
    [SerializeField] private List<GameObject> secondLevelJunk = new List<GameObject>();
    [SerializeField] private List<GameObject> thirdLevelJunk = new List<GameObject>();
    [SerializeField] private GameObject teamPlastic, teamPaper, teamGlass, center;
    [SerializeField] private GameObject[] sceneToSceneFastMovingObjects;
    [SerializeField] private GameObject scene01, scene02, scene03;
    Coroutine sceneToSceneTransition;
    private const int sceneToSceneLenght = 39;
    private const int allScenesJump = 117;
    private int jungleLevel = 1;
    private int levelsPlayed;
    [SerializeField] private Animator pauseMenu;
    [SerializeField] private Animator tutorialHand;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text highScoreNameText;
    [SerializeField] private Animator succesfullImage;
    [SerializeField] private Animator redScreenOfDeath;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip[] clips;
    private int firstPlay;
    [HideInInspector] public bool tutorial, glassTutorial, plasticTutorial, paperTutorial;
    private GameObject tutorialGlassPlacement, tutorialPaperPlacement, tutorialPlasticPlacement;
    [SerializeField] private GameObject glassHand01, glassHand02, glassHand03;
    [SerializeField] private GameObject plasticHand01, plasticHand02, plasticHand03;
    [SerializeField] private GameObject paperHand01, paperHand02, paperHand03;
    [SerializeField] private float tutorialHandSpeed, tutorialHandSpeedTwo;
    [HideInInspector] public Coroutine tutorialCoroutine;
    public static JungleLevel Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        firstPlay = PlayerPrefs.GetInt("JungleFirstPlay");
        highScore = PlayerPrefs.GetInt("IndiaHighScore");
        highScoreName = PlayerPrefs.GetString("IndiaHighScoreName");
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
        StartLittering();
        if (firstPlay == 1)
        {
            StartGameWithoutTutorial();
        }
        else
        {
            PlacementForTutorial();
            Invoke("Tutorial", 0.1f);
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

    public void ReduceLives()
    {
        if (lives > 0)
        {
            lives--;
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
                CheckIfPlayedThreeLevels();
            }
        }
    }
    private void CheckIfPlayedThreeLevels()
    {
        if (levelsPlayed > 2)
        {
            EverybodyHalt();
            PlayerPrefs.SetInt("IndiaScore", scorePoints);
            StartCoroutine(SceneTransition("jungle_good"));
        }
        else
        {
            EverybodyHalt();
            PlayerPrefs.SetInt("IndiaScore", scorePoints);
            StartCoroutine(SceneTransition("jungle_bad"));
        }
    }
    private void EverybodyHalt()
    {
        TeamPaper.Instance.Finished();
        TeamGlass.Instance.Finished();
        TeamPlastic.Instance.Finished();
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
    public void CheckIfOkToTransitionToNextScene()
    {
        if (TeamPaper.Instance.isFinished && TeamPlastic.Instance.isFinished && TeamGlass.Instance.isFinished)
        {
            Invoke("StartTransition", JungleTimer.Instance.restartTime / 2);
            if(levelsPlayed == 3)
            {
                StartCoroutine(AnimateSuccesfullImage());
                WinAudio();
            }
        }
    }
    private void StartTransition()
    {
        StopMusic();
        StartTruckAudio();
        AllTeamsDrive();
        sceneToSceneTransition = StartCoroutine(SceneToSceneTransition());
        QuickSceneToSceneTransition();
        StartCoroutine(SceneJump());
    }
    IEnumerator SceneToSceneTransition()
    {
        Vector3 teamPlasticNewPosition = new Vector3(teamPlastic.transform.position.x + sceneToSceneLenght, teamPlastic.transform.position.y, teamPlastic.transform.position.z);
        Vector3 teamPaperNewPosition = new Vector3(teamPaper.transform.position.x + sceneToSceneLenght, teamPaper.transform.position.y, teamPaper.transform.position.z);
        Vector3 teamGlassNewPosition = new Vector3(teamGlass.transform.position.x + sceneToSceneLenght, teamGlass.transform.position.y, teamGlass.transform.position.z);
        Vector3 centerNewPosition = new Vector3(center.transform.position.x + sceneToSceneLenght, center.transform.position.y, center.transform.position.z);
        while(center.transform.position != centerNewPosition)
        {
            teamPlastic.transform.position = Vector2.MoveTowards(teamPlastic.transform.position, teamPlasticNewPosition, JungleTimer.Instance.sceneToSceneTransitionSpeed * Time.deltaTime);
            teamPaper.transform.position = Vector2.MoveTowards(teamPaper.transform.position, teamPaperNewPosition, JungleTimer.Instance.sceneToSceneTransitionSpeed * Time.deltaTime);
            teamGlass.transform.position = Vector2.MoveTowards(teamGlass.transform.position, teamGlassNewPosition, JungleTimer.Instance.sceneToSceneTransitionSpeed * Time.deltaTime);
            center.transform.position = Vector2.MoveTowards(center.transform.position, centerNewPosition, JungleTimer.Instance.sceneToSceneTransitionSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    private void QuickSceneToSceneTransition()
    {
        foreach(GameObject movingObject in sceneToSceneFastMovingObjects)
        {
            movingObject.transform.position = new Vector3(movingObject.transform.position.x + sceneToSceneLenght, movingObject.transform.position.y, movingObject.transform.position.z);
        }
        jungleLevel++;
        levelsPlayed++;
        if (jungleLevel > 3)
            jungleLevel = 1;
    }
    IEnumerator SceneJump()
    {
        yield return sceneToSceneTransition;
        if(jungleLevel == 1)
        {
            scene03.transform.position = new Vector3(scene03.transform.position.x + allScenesJump, scene03.transform.position.y, scene03.transform.position.z);
            LitterTheThirdLevel();
            JungleTimer.Instance.Accelerate();
            AllTeamsGetOut();
            audioSource.Stop();
            DoorSlamAudio();
            Invoke("StartNewScene", JungleTimer.Instance.restartTime / 2);
        }
        else if (jungleLevel == 2)
        {
            scene01.transform.position = new Vector3(scene01.transform.position.x + allScenesJump, scene01.transform.position.y, scene01.transform.position.z);
            LitterTheFirstLevel();
            JungleTimer.Instance.Accelerate();
            AllTeamsGetOut();
            audioSource.Stop();
            DoorSlamAudio();
            Invoke("StartNewScene", JungleTimer.Instance.restartTime / 2);
        }
        else if (jungleLevel == 3)
        {
            scene02.transform.position = new Vector3(scene02.transform.position.x + allScenesJump, scene02.transform.position.y, scene02.transform.position.z);
            LitterTheSecondLevel();
            JungleTimer.Instance.Accelerate();
            AllTeamsGetOut();
            audioSource.Stop();
            DoorSlamAudio();
            Invoke("StartNewScene", JungleTimer.Instance.restartTime / 2);
        }
    }
    private void StartNewScene()
    {
        PlayMusic();
        TeamPlastic.Instance.NewLevel();
        TeamPaper.Instance.NewLevel();
        TeamGlass.Instance.NewLevel();
    }
    private void StartLittering()
    {
        Shuffle(levelFirstSpawnPoints);
        Shuffle(levelSecondSpawnPoints);
        Shuffle(levelThirdSpawnPoints);
        for (int i = 0; i < levelFirstSpawnPoints.Count; i++)
        {
            firstLevelJunk[i].transform.position = levelFirstSpawnPoints[i].transform.position;
            if (firstLevelJunk[i].tag == "plastic")
                firstLevelJunk[i].GetComponent<Plastic>().resetPostion = levelFirstSpawnPoints[i].transform.position;
            else if (firstLevelJunk[i].tag == "paper")
                firstLevelJunk[i].GetComponent<Paper>().resetPostion = levelFirstSpawnPoints[i].transform.position;
            else if (firstLevelJunk[i].tag == "glass")
                firstLevelJunk[i].GetComponent<Glass>().resetPostion = levelFirstSpawnPoints[i].transform.position;
        }
        for (int i = 0; i < levelSecondSpawnPoints.Count; i++)
        {
            secondLevelJunk[i].transform.position = levelSecondSpawnPoints[i].transform.position;
            if (secondLevelJunk[i].tag == "plastic")
                secondLevelJunk[i].GetComponent<Plastic>().resetPostion = levelSecondSpawnPoints[i].transform.position;
            else if (secondLevelJunk[i].tag == "paper")
                secondLevelJunk[i].GetComponent<Paper>().resetPostion = levelSecondSpawnPoints[i].transform.position;
            else if (secondLevelJunk[i].tag == "glass")
                secondLevelJunk[i].GetComponent<Glass>().resetPostion = levelSecondSpawnPoints[i].transform.position;
        }
        for (int i = 0; i < levelThirdSpawnPoints.Count; i++)
        {
            thirdLevelJunk[i].transform.position = levelThirdSpawnPoints[i].transform.position;
            if (thirdLevelJunk[i].tag == "plastic")
                thirdLevelJunk[i].GetComponent<Plastic>().resetPostion = levelThirdSpawnPoints[i].transform.position;
            else if (thirdLevelJunk[i].tag == "paper")
                thirdLevelJunk[i].GetComponent<Paper>().resetPostion = levelThirdSpawnPoints[i].transform.position;
            else if (thirdLevelJunk[i].tag == "glass")
                thirdLevelJunk[i].GetComponent<Glass>().resetPostion = levelThirdSpawnPoints[i].transform.position;
        }
    }
    private void PlacementForTutorial()
    {
        foreach( GameObject junk in firstLevelJunk)
        {
            if (junk.name == TeamPlastic.Instance.junk[0].tag)
                tutorialPlasticPlacement = junk;
            if (junk.name == TeamPaper.Instance.junk[0].tag)
                tutorialPaperPlacement = junk;
            if (junk.name == TeamGlass.Instance.junk[0].tag)
                tutorialGlassPlacement = junk;
        }
        plasticHand02.transform.position = new Vector3(tutorialPlasticPlacement.transform.position.x - 1.5f, tutorialPlasticPlacement.transform.position.y - 1.5f, tutorialPlasticPlacement.transform.position.z);
        plasticHand01.transform.position = new Vector3(plasticHand02.transform.position.x - 1.5f, plasticHand02.transform.position.y - 1f, plasticHand02.transform.position.z);
        paperHand02.transform.position = new Vector3(tutorialPaperPlacement.transform.position.x - 1f, tutorialPaperPlacement.transform.position.y - 1.5f, tutorialPaperPlacement.transform.position.z);
        paperHand01.transform.position = new Vector3(paperHand02.transform.position.x - 1.5f, paperHand02.transform.position.y - 1f, paperHand02.transform.position.z);
        glassHand02.transform.position = new Vector3(tutorialGlassPlacement.transform.position.x - 1f, tutorialGlassPlacement.transform.position.y - 1.5f, tutorialGlassPlacement.transform.position.z);
        glassHand01.transform.position = new Vector3(glassHand02.transform.position.x - 1.5f, glassHand02.transform.position.y - 1f, glassHand02.transform.position.z);
    }
    private void LitterTheFirstLevel()
    {
        Shuffle(levelFirstSpawnPoints);
        for (int i = 0; i < levelFirstSpawnPoints.Count; i++)
        {
            firstLevelJunk[i].transform.position = levelFirstSpawnPoints[i].transform.position;
            if (firstLevelJunk[i].tag == "plastic")
                firstLevelJunk[i].GetComponent<Plastic>().resetPostion = levelFirstSpawnPoints[i].transform.position;
            else if (firstLevelJunk[i].tag == "paper")
                firstLevelJunk[i].GetComponent<Paper>().resetPostion = levelFirstSpawnPoints[i].transform.position;
            else if (firstLevelJunk[i].tag == "glass")
                firstLevelJunk[i].GetComponent<Glass>().resetPostion = levelFirstSpawnPoints[i].transform.position;
            firstLevelJunk[i].SetActive(true);
        }
    }
    private void LitterTheSecondLevel()
    {
        Shuffle(levelSecondSpawnPoints);
        for (int i = 0; i < levelSecondSpawnPoints.Count; i++)
        {
            secondLevelJunk[i].transform.position = levelSecondSpawnPoints[i].transform.position;
            if (secondLevelJunk[i].tag == "plastic")
                secondLevelJunk[i].GetComponent<Plastic>().resetPostion = levelSecondSpawnPoints[i].transform.position;
            else if (secondLevelJunk[i].tag == "paper")
                secondLevelJunk[i].GetComponent<Paper>().resetPostion = levelSecondSpawnPoints[i].transform.position;
            else if (secondLevelJunk[i].tag == "glass")
                secondLevelJunk[i].GetComponent<Glass>().resetPostion = levelSecondSpawnPoints[i].transform.position;
            secondLevelJunk[i].SetActive(true);
        }
    }
    private void LitterTheThirdLevel()
    {
        Shuffle(levelThirdSpawnPoints);
        for (int i = 0; i < levelThirdSpawnPoints.Count; i++)
        {
            thirdLevelJunk[i].transform.position = levelThirdSpawnPoints[i].transform.position;
            if (thirdLevelJunk[i].tag == "plastic")
                thirdLevelJunk[i].GetComponent<Plastic>().resetPostion = levelThirdSpawnPoints[i].transform.position;
            else if (thirdLevelJunk[i].tag == "paper")
                thirdLevelJunk[i].GetComponent<Paper>().resetPostion = levelThirdSpawnPoints[i].transform.position;
            else if (thirdLevelJunk[i].tag == "glass")
                thirdLevelJunk[i].GetComponent<Glass>().resetPostion = levelThirdSpawnPoints[i].transform.position;
            thirdLevelJunk[i].SetActive(true);
        }
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
    private void StartTruckAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "truck")
            {
                audioSource.clip = clip;
                audioSource.Play();
            }

        }
    }
    private void DoorSlamAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "doorSlam")
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
        musicAudioSource.pitch = JungleTimer.Instance.musicPitch;
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
        SceneManager.LoadScene("jungle_intro");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("menu_games");
    }
    private void AllTeamsDrive()
    {
        TeamPlastic.Instance.SetCharacterState("kamion_vozi");
        TeamPaper.Instance.SetCharacterState("kamion_vozi");
        TeamGlass.Instance.SetCharacterState("kamion_vozi");
    }
    private void AllTeamsGetOut()
    {
        TeamPlastic.Instance.SetCharacterState("kamion_dolazak_priprema");
        TeamPaper.Instance.SetCharacterState("kamion_dolazak_priprema");
        TeamGlass.Instance.SetCharacterState("kamion_dolazak_priprema");
    }
    private void StartGameWithoutTutorial()
    {
        tutorial = false;
        TeamPlastic.Instance.NewLevel();
        TeamGlass.Instance.NewLevel();
        TeamPaper.Instance.NewLevel();
    }
    private void Tutorial()
    {
        tutorialCoroutine = StartCoroutine(PlasticTutorial());
        tutorial = true;
    }
    private IEnumerator PlasticTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = plasticHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        plasticTutorial = true;
        TeamPlastic.Instance.AskForJunkTutorial();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != plasticHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, plasticHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = plasticHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != plasticHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, plasticHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = plasticHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != plasticHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, plasticHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = plasticHand02.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != plasticHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, plasticHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = plasticHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != plasticHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, plasticHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = plasticHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != plasticHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, plasticHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = plasticHand01.transform.position;
        }
    }
    public void StartPaperTutorial()
    {
        plasticTutorial = false;
        StopCoroutine(tutorialCoroutine);
        tutorialHand.SetBool("isOpen", false);
        tutorialCoroutine = StartCoroutine(PaperTutorial());
    }
    private IEnumerator PaperTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = paperHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        paperTutorial = true;
        TeamPaper.Instance.AskForJunkTutorial();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != paperHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, paperHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = paperHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != paperHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, paperHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = paperHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != paperHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, paperHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = paperHand02.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != paperHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, paperHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = paperHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != paperHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, paperHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = paperHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != paperHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, paperHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = paperHand01.transform.position;
        }
    }
    public void StartGlassTutorial()
    {
        paperTutorial = false;
        StopCoroutine(tutorialCoroutine);
        tutorialHand.SetBool("isOpen", false);
        tutorialCoroutine = StartCoroutine(GlassTutorial());
    }
    private IEnumerator GlassTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorialHand.gameObject.transform.position = glassHand01.transform.position;
        tutorialHand.SetBool("isOpen", true);
        glassTutorial = true;
        TeamGlass.Instance.AskForJunkTutorial();
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != glassHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, glassHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = glassHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != glassHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, glassHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = glassHand01.transform.position;
            while (tutorialHand.gameObject.transform.position != glassHand02.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, glassHand02.transform.position, tutorialHandSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = glassHand02.transform.position;

            yield return new WaitForSeconds(1);
            while (tutorialHand.gameObject.transform.position != glassHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, glassHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = glassHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != glassHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, glassHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = glassHand02.transform.position;
            while (tutorialHand.gameObject.transform.position != glassHand03.transform.position)
            {
                tutorialHand.gameObject.transform.position = Vector2.MoveTowards(tutorialHand.gameObject.transform.position, glassHand03.transform.position, tutorialHandSpeedTwo * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            tutorialHand.gameObject.transform.position = glassHand01.transform.position;
        }
    }
    public void EndTutorial()
    {
        glassTutorial = false;
        StopCoroutine(tutorialCoroutine);
        PlayerPrefs.SetInt("JungleFirstPlay", 1);
        tutorialHand.SetBool("isOpen", false);
        Invoke("ContinueLevelAfterTutorial", 1f);
    }
    private void ContinueLevelAfterTutorial()
    {
        tutorial = false;
        TeamPlastic.Instance.ContinueLevel();
        TeamPaper.Instance.ContinueLevel();
        TeamGlass.Instance.ContinueLevel();
    }
   
}
