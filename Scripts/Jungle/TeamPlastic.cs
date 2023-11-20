using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TeamPlastic : MonoBehaviour
{
    public SkeletonAnimation animator;
    public Animator speechBubbleAnimator; 
    [SerializeField] private AnimationReferenceAsset kamion_dolazak_priprema, kamion_odlazak_priprema, kamion_request, kamion_stoje, kamion_ubacuje, kamion_vozi;
    private string currentAnimation = "kamion_stoje";
    private bool currentAnimationBool;
    [SerializeField] private float workStartDelay;
    public List<GameObject> junk = new List<GameObject>();
    [HideInInspector] public bool askForJunk;
    private Coroutine currentCoroutine;
    private int junkIndex;
    [HideInInspector] public bool isFinished;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips;
    private SpriteRenderer speechBubbleSpriteRenderer;
    private Color startColor, endColor;
    private Coroutine changeColorCouroutine;
    public static TeamPlastic Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("Sounds") == 0)
            audioSource.mute = true;
        speechBubbleSpriteRenderer = speechBubbleAnimator.transform.gameObject.GetComponent<SpriteRenderer>();
        startColor = Color.white;
        endColor = Color.red;
        CloseAllItems();
        animator.AnimationState.Event += OnMyEvent;
        if(JungleLevel.Instance.tutorial)
        {
            Shuffle(junk);
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
    public void NewLevel()
    {
        junkIndex = 0;
        isFinished = false;
        Invoke("StartWork", workStartDelay);
        Shuffle(junk);
    }
    public void ContinueLevel()
    {
        Invoke("StartWork", workStartDelay);
    }
    public void StartWork()
    {
        StopAllCoroutines();
        currentCoroutine = StartCoroutine(AskForJunk());
    }
    private IEnumerator AskForJunk()
    {
        askForJunk = true;
        changeColorCouroutine = StartCoroutine(ChangeSpeechBubbleColour());
        ShowSpeechBubble();
        Invoke("ShowJunk", 0.25f);
        SetCharacterState("kamion_request");
        yield return new WaitForSeconds(JungleTimer.Instance.askingTime);
        askForJunk = false;
        StopCoroutine(changeColorCouroutine);
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        CloseAllItems();
        SetCharacterState("kamion_stoje");
        Invoke("CloseSpeechBubble", 0.1f);
        JungleLevel.Instance.ReduceLives();
        Invoke("StartWork", JungleTimer.Instance.restartTime);
    }
    public void ThrowJunk()
    {
        askForJunk = false;
        StopCoroutine(changeColorCouroutine);
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        StopCoroutine(currentCoroutine);
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        SetCharacterState("kamion_ubacuje");
        currentCoroutine = StartCoroutine(ThrowingJunk());
        CollectPlasticAudio();
    }
    private IEnumerator ThrowingJunk()
    {
        yield return new WaitForSeconds(JungleTimer.Instance.workingTime);
        junkIndex++;
        CheckIfFinished();
    }
    private IEnumerator ChangeSpeechBubbleColour()
    {
        yield return new WaitForSeconds(JungleTimer.Instance.askingTime / 2);
        float t = JungleTimer.Instance.askingTime / 2;
        float smoothness = JungleTimer.Instance.speechBubbleColorSmoothness;
        float progress = 0;
        float increment = smoothness / t;
        while (progress < 1)
        {
            speechBubbleSpriteRenderer.color = Color.Lerp(startColor, endColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
    private void CheckIfFinished()
    {
        if(junkIndex >= junk.Count)
        {
            isFinished = true;
            SetCharacterState("kamion_odlazak_priprema");
            JungleLevel.Instance.CheckIfOkToTransitionToNextScene();
        }
        else if(!JungleLevel.Instance.tutorial)
        {
            Invoke("StartWork", JungleTimer.Instance.restartTime);
        }
    }
    public void Finished()
    {
        CloseAllItems();
        SetCharacterState("kamion_stoje");
        Invoke("CloseSpeechBubble", 0.1f);
    }
    private void CloseAllItems()
    {
      foreach (GameObject junk in junk)
        {
            junk.gameObject.SetActive(false);
        }
    }
    private void ShowJunk()
    {
        junk[junkIndex].SetActive(true);
    }
    private void ShowSpeechBubble()
    {
        speechBubbleAnimator.SetBool("isOpen", true);
    }
    private void CloseSpeechBubble()
    {
        speechBubbleAnimator.SetBool("isOpen", false);
    }
    private void ChangeSpeechBubbleColorToNormal()
    {
        speechBubbleSpriteRenderer.color = startColor;
    }
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        if (animationName.name.Equals(currentAnimation))
            return;
        animator.state.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
        currentAnimationBool = loop;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("kamion_dolazak_priprema"))
        {
            SetAnimation(kamion_dolazak_priprema, false, JungleTimer.Instance.animationSpeed);
            AddAnimation(kamion_stoje, true, JungleTimer.Instance.animationSpeed);
        }
        if (state.Equals("kamion_odlazak_priprema"))
        {
            SetAnimation(kamion_odlazak_priprema, false, JungleTimer.Instance.animationSpeed);
        }
        if (state.Equals("kamion_request"))
        {
            SetAnimation(kamion_request, true, JungleTimer.Instance.animationSpeed);
        }
        if (state.Equals("kamion_stoje"))
        {
            SetAnimation(kamion_stoje, true, JungleTimer.Instance.animationSpeed);
        }
        if (state.Equals("kamion_ubacuje"))
        {
            SetAnimation(kamion_ubacuje, false, JungleTimer.Instance.animationSpeed);
            AddAnimation(kamion_stoje, true, JungleTimer.Instance.animationSpeed);
        }
        if (state.Equals("kamion_vozi"))
        {
            SetAnimation(kamion_vozi, true, JungleTimer.Instance.animationSpeed);
        }
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
    }
    public void AccelerateAnimation()
    {
        animator.state.SetAnimation(0, currentAnimation, currentAnimationBool).TimeScale = JungleTimer.Instance.animationSpeed / 0.3f;
        speechBubbleAnimator.SetBool("isTriggered", true);
    }
    public void ReturnAnimationToNormalSpeed()
    {
        animator.state.SetAnimation(0, currentAnimation, currentAnimationBool).TimeScale = JungleTimer.Instance.animationSpeed;
        if (!currentAnimationBool)
            AddAnimation(kamion_stoje, true, FloodTimer.Instance.animationSpeed);
        speechBubbleAnimator.SetBool("isTriggered", false);
    }
    void OnMyEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "baghit")
        {
            DumpAudio();
            JungleLevel.Instance.ScorePointAudio();
            JungleLevel.Instance.AddScorePoints();
        }
    }
    private void CollectPlasticAudio()
    {
        string[] digSounds = new string[] { "plastika1", "plastika2", "plastika3" };
        string randomString = digSounds[Random.Range(0, digSounds.Length)];
        foreach (AudioClip clip in clips)
        {

            if (clip.name == randomString)
                audioSource.PlayOneShot(clip);
        }
    }
    private void DumpAudio()
    {
        string[] digSounds = new string[] { "dump1", "dump2"};
        string randomString = digSounds[Random.Range(0, digSounds.Length)];
        foreach (AudioClip clip in clips)
        {

            if (clip.name == randomString)
                audioSource.PlayOneShot(clip);
        }
    }
    public void AskForJunkTutorial()
    {
        ShowSpeechBubble();
        askForJunk = true;
        Invoke("ShowJunk", 0.25f);
        SetCharacterState("kamion_request");
    }
    public void ThrowJunkTutorial()
    {
        askForJunk = false;
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        SetCharacterState("kamion_ubacuje");
        currentCoroutine = StartCoroutine(ThrowingJunk());
        CollectPlasticAudio();
    }
}
