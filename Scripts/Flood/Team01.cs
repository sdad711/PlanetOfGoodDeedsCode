using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Team01 : MonoBehaviour
{
    public SkeletonAnimation animator;
    private PolygonCollider2D characterCollider;
    public Animator speechBubbleAnimator;
    public Sand sand;
    [SerializeField] private AnimationReferenceAsset digging, facepalm, iznenadi_se, panic1, panic2, panic3, puni_vrecu, request, request2, request3, standing, standing_idle1, standing_idle2, veze_vrecu, winDance1, winDance2, winDance3;
    private string currentAnimation = "standing";
    private bool currentAnimationBool;
    [SerializeField] private float workStartDelay;
    [SerializeField] private GameObject showel, bag, rope, sandBag;
    private CircleCollider2D sandBagCollider;
    private SpriteRenderer sandBagSpriteRenderer;
    private SpriteRenderer speechBubbleSpriteRenderer;
    [HideInInspector] public bool askForShowel, askForBag, askForRope;
    private Coroutine currentCoroutine;
    private string coroutineName;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips;
    private Color startColor, endColor;
    private Coroutine changeColorCouroutine;
    public static Team01 Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("Sounds") == 0)
            audioSource.mute = true;
        characterCollider = GetComponent<PolygonCollider2D>();
        sandBagCollider = sandBag.GetComponent<CircleCollider2D>();
        sandBagSpriteRenderer = sandBag.GetComponent<SpriteRenderer>();
        speechBubbleSpriteRenderer = speechBubbleAnimator.transform.gameObject.GetComponent<SpriteRenderer>();
        startColor = Color.white;
        endColor = Color.red;
        CloseAllItems();
    }
    public void NewWave()
    {
        Invoke("StartWork", workStartDelay);
    }
    public void StartWork()
    {
        StopAllCoroutines();
        GameObject speechBubble;
        speechBubble = speechBubbleAnimator.gameObject;
        speechBubble.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        currentCoroutine = StartCoroutine(AskForShowel());
        coroutineName = "AskForShowel";
    }
    private IEnumerator AskForShowel()
    {
        askForShowel = true;
        changeColorCouroutine = StartCoroutine(ChangeSpeechBubbleColour());
        ShowSpeechBubble();
        Invoke("ShowShowel", 0.25f);
        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
            SetCharacterState("request");
        else if (randomInt == 1)
            SetCharacterState("request2");
        else if (randomInt == 2)
            SetCharacterState("request3");
        yield return new WaitForSeconds(FloodTimer.Instance.askingTime);
        askForShowel = false;
        StopCoroutine(changeColorCouroutine);
        CloseAllItems();
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        randomInt = Random.Range(0, 2);
        if (randomInt == 0)
            SetCharacterState("facepalm");
        else if (randomInt == 1)
            SetCharacterState("iznenadi_se");
        Invoke("CloseSpeechBubble", 0.1f);
        FloodLevel.Instance.ReduceLives();
        Invoke("Error", FloodTimer.Instance.restartTime);
    }
    public void StartDiggingSand()
    {
        askForShowel = false;
        StopCoroutine(changeColorCouroutine);
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        StopCoroutine(currentCoroutine);
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        SetCharacterState("digging");
        sand.SetCharacterState("triFaze");
        currentCoroutine = StartCoroutine(DigSand());
        coroutineName = "DigSand";
        DigSandAudio();
    }
    private IEnumerator DigSand()
    {
        yield return new WaitForSeconds(FloodTimer.Instance.workingTime);
        currentCoroutine = StartCoroutine(AskForBag());
        coroutineName = "AskForBag";
    }
    public void StartAskingBag()
    {
        StopAllCoroutines();
        currentCoroutine = StartCoroutine(AskForBag());
        coroutineName = "AskForBag";
    }
    private IEnumerator AskForBag()
    {
        askForBag = true;
        changeColorCouroutine = StartCoroutine(ChangeSpeechBubbleColour());
        ShowSpeechBubble();
        Invoke("ShowBag", 0.25f);
        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
            SetCharacterState("request");
        else if (randomInt == 1)
            SetCharacterState("request2");
        else if (randomInt == 2)
            SetCharacterState("request3");
        yield return new WaitForSeconds(FloodTimer.Instance.askingTime);
        askForBag = false;
        StopCoroutine(changeColorCouroutine);
        CloseAllItems();
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        randomInt = Random.Range(0, 2);
        if (randomInt == 0)
            SetCharacterState("facepalm");
        else if (randomInt == 1)
            SetCharacterState("iznenadi_se");
        Invoke("CloseSpeechBubble", 0.1f);
        FloodLevel.Instance.ReduceLives();
        Invoke("Error", FloodTimer.Instance.restartTime);
    }
    public void StartPuttingSandInBag()
    {
        askForBag = false;
        StopCoroutine(changeColorCouroutine);
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        StopCoroutine(currentCoroutine);
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        SetCharacterState("puni_vrecu");
        sand.SetCharacterState("triFaze_unatrag");
        currentCoroutine = StartCoroutine(PutSandInBag());
        coroutineName = "PutSandInBag";
        OpenBagAudio();
    }
    private IEnumerator PutSandInBag()
    {
        yield return new WaitForSeconds(FloodTimer.Instance.workingTime);
        currentCoroutine = StartCoroutine(AskForRope());
        coroutineName = "AskForRope";
    }
    public void StartAskingForRope()
    {
        StopAllCoroutines();
        currentCoroutine = StartCoroutine(AskForRope());
        coroutineName = "AskForRope";
    }
    private IEnumerator AskForRope()
    {
        askForRope = true;
        changeColorCouroutine = StartCoroutine(ChangeSpeechBubbleColour());
        ShowSpeechBubble();
        Invoke("ShowRope", 0.25f);
        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
            SetCharacterState("request");
        else if (randomInt == 1)
            SetCharacterState("request2");
        else if (randomInt == 2)
            SetCharacterState("request3");
        yield return new WaitForSeconds(FloodTimer.Instance.askingTime);
        askForRope = false;
        StopCoroutine(changeColorCouroutine);
        CloseAllItems();
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        randomInt = Random.Range(0, 2);
        if (randomInt == 0)
            SetCharacterState("facepalm");
        else if (randomInt == 1)
            SetCharacterState("iznenadi_se");
        Invoke("CloseSpeechBubble", 0.1f);
        FloodLevel.Instance.ReduceLives();
        Invoke("Error", FloodTimer.Instance.restartTime);
    }
    public void StartTyingRope()
    {
        askForRope = false;
        StopCoroutine(changeColorCouroutine);
        Invoke("ChangeSpeechBubbleColorToNormal", 0.2f);
        StopCoroutine(currentCoroutine);
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        SetCharacterState("veze_vrecu");
        currentCoroutine = StartCoroutine(TyingRope());
        coroutineName = "TyingRope";
        ZipBagAudio();
    }
    private IEnumerator TyingRope()
    {
        yield return new WaitForSeconds(FloodTimer.Instance.workingTime);
        SandBagBuild();
    }
    private IEnumerator ChangeSpeechBubbleColour()
    {
        yield return new WaitForSeconds(FloodTimer.Instance.askingTime / 2);
        float t = FloodTimer.Instance.askingTime / 2;
        float smoothness = FloodTimer.Instance.speechBubbleColorSmoothness;
        float progress = 0;
        float increment = smoothness / t;
        while(progress < 1)
        {
            speechBubbleSpriteRenderer.color = Color.Lerp(startColor, endColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
    private void SandBagBuild()
    {
        coroutineName = "SandBagBuild";
        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
            SetCharacterState("winDance1");
        else if (randomInt == 1)
            SetCharacterState("winDance2");
        else if (randomInt == 2)
            SetCharacterState("winDance3");
        GameObject speechBubble;
        speechBubble = speechBubbleAnimator.gameObject;
        speechBubble.GetComponent<SpriteRenderer>().color = new Color32(44, 78, 180, 255);
        ShowSpeechBubble();
        Invoke("ShowSandBag", 0.25f);
        Invoke("WallBuilderAsksForSandBag", 0.1f);
        Invoke("WallBuilderAsksForSandBag", 0.2f);
        SandBagBuildAudio();
        characterCollider.enabled = false;
    }
    public void Finished()
    {
        coroutineName = "Finished";
        CloseAllItems();
        int randomInt = Random.Range(0, 2);
        if (randomInt == 0)
            SetCharacterState("standing_idle1");
        else if (randomInt == 1)
            SetCharacterState("standing_idle2");
        Invoke("CloseSpeechBubble", 0.1f);
        Invoke("StartWork", FloodTimer.Instance.restartTime);
        ScorePointAudio();
        characterCollider.enabled = true;
    }
    public void Error()
    {
        StopAllCoroutines();
        CancelInvoke();
        switch (coroutineName)
        {
            case "AskForShowel":
                StartWork();
                break;
            case "DigSand":
                sand.SetCharacterState("prazno");
                StartDiggingSand();
                break;
            case "AskForBag":
                StartAskingBag();
                sand.SetCharacterState("puno");
                break;
            case "PutSandInBag":
                StartPuttingSandInBag();
                sand.SetCharacterState("puno");
                break;
            case "AskForRope":
                StartAskingForRope();
                break;
            case "TyingRope":
                StartTyingRope();
                break;
            case "SandBagBuild":
                SandBagBuild();
                break;
            case "Finished":
                StartWork();
                break;
        }
    }
    public void Panic()
    {
        StopAllCoroutines();
        CancelInvoke();
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
            SetCharacterState("panic1");
        else if (randomInt == 1)
            SetCharacterState("panic2");
        else if (randomInt == 2)
            SetCharacterState("panic3");
        sand.SetCharacterState("prazno");
    }
    public void Chill()
    {
        StopAllCoroutines();
        CancelInvoke();
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        int randomInt = Random.Range(0, 3);
        if (randomInt == 0)
            SetCharacterState("winDance1");
        else if (randomInt == 1)
            SetCharacterState("winDance2");
        else if (randomInt == 2)
            SetCharacterState("winDance3");
        sand.SetCharacterState("prazno");
    }
    public void GetBackToWork()
    {
        int randomInt = Random.Range(0, 2);
        if (randomInt == 0)
            SetCharacterState("iznenadi_se");
        else if (randomInt == 1)
            SetCharacterState("facepalm");
        Invoke("Error", workStartDelay / 2);
        //Error();
    }
    private void CloseAllItems()
    {
        showel.gameObject.SetActive(false);
        bag.gameObject.SetActive(false);
        rope.gameObject.SetActive(false);
        sandBagCollider.enabled = false;
        sandBagSpriteRenderer.enabled = false;
    }
    private void ShowShowel()
    {
        showel.gameObject.SetActive(true);
    }
    private void ShowBag()
    {
        bag.gameObject.SetActive(true);
    }
    private void ShowRope()
    {
        rope.gameObject.SetActive(true);
    }
    private void ShowSandBag()
    {
        sandBagCollider.enabled = true;
        sandBagSpriteRenderer.enabled = true;
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
        if (state.Equals("digging"))
        {
            SetAnimation(digging, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("facepalm"))
        {
            SetAnimation(facepalm, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(standing, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("iznenadi_se"))
        {
            SetAnimation(iznenadi_se, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(standing, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("panic1"))
        {
            SetAnimation(panic1, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("panic2"))
        {
            SetAnimation(panic2, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("panic3"))
        {
            SetAnimation(panic3, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("puni_vrecu"))
        {
            SetAnimation(puni_vrecu, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("request"))
        {
            SetAnimation(request, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("request2"))
        {
            SetAnimation(request2, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("request3"))
        {
            SetAnimation(request3, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("standing"))
        {
            SetAnimation(standing, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("standing_idle1"))
        {
            SetAnimation(standing_idle1, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(standing, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("standing_idle2"))
        {
            SetAnimation(standing_idle2, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(standing, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("veze_vrecu"))
        {
            SetAnimation(veze_vrecu, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("winDance1"))
        {
            SetAnimation(winDance1, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("winDance2"))
        {
            SetAnimation(winDance2, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("winDance3"))
        {
            SetAnimation(winDance3, true, FloodTimer.Instance.animationSpeed);
        }
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
    }
    public void AccelerateAnimation()
    {
        animator.state.SetAnimation(0, currentAnimation, currentAnimationBool).TimeScale = FloodTimer.Instance.animationSpeed / 0.3f;
        speechBubbleAnimator.SetBool("isTriggered", true);
    }
    public void ReturnAnimationToNormalSpeed()
    {
        animator.state.SetAnimation(0, currentAnimation, currentAnimationBool).TimeScale = FloodTimer.Instance.animationSpeed;
        if (!currentAnimationBool)
            AddAnimation(standing, true, FloodTimer.Instance.animationSpeed);
        speechBubbleAnimator.SetBool("isTriggered", false);
    }
    private void WallBuilderAsksForSandBag()
    {
        FloodLevel.Instance.WallBuilderAsksForSandBag();
    }
    private void DigSandAudio()
    {
        string[] digSounds = new string[] { "dig1", "dig2", "dig3" };
        string randomString = digSounds[Random.Range(0, digSounds.Length)];
        foreach (AudioClip clip in clips)
        {
            
            if (clip.name == randomString)
                audioSource.PlayOneShot(clip);
        }
    }
    private void OpenBagAudio()
    {
        string[] bagSounds = new string[] { "bag1", "bag2", "bag3" };
        string randomString = bagSounds[Random.Range(0, bagSounds.Length)];
        foreach (AudioClip clip in clips)
        {

            if (clip.name == randomString)
                audioSource.PlayOneShot(clip);
        }
    }
    private void ZipBagAudio()
    {
        string[] zipSounds = new string[] { "zip1", "zip2", "zip3" };
        string randomString = zipSounds[Random.Range(0, zipSounds.Length)];
        foreach (AudioClip clip in clips)
        {

            if (clip.name == randomString)
                audioSource.PlayOneShot(clip);
        }
    }
    private void SandBagBuildAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "ding")
                audioSource.PlayOneShot(clip);
        }
    }
    private void ScorePointAudio()
    {
        foreach (AudioClip clip in clips)
        {

            if (clip.name == "collectPoint")
                audioSource.PlayOneShot(clip);
        }
    }
    public void TutorialAskShowel()
    {
        ShowSpeechBubble();
        Invoke("ShowShowel", 0.25f);
        SetCharacterState("request");
        //audio
    }
    public void TutorialShowelRequired()
    {
        CloseAllItems();
        Invoke("CloseSpeechBubble", 0.1f);
        SetCharacterState("digging");
        sand.SetCharacterState("triFaze");
        DigSandAudio();
    }
    public void ResetSandToEmpty()
    {
        sand.SetCharacterState("prazno");
    }
}