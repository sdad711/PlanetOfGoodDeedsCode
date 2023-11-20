using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class WallBuilder : MonoBehaviour
{
    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset hoda, idle1, idle2, panika, stoji, veselje, vreca_pojaviseNestane, vreca_uzmestavi, vreca_winBaci, zove;
    private string currentAnimation;
    [HideInInspector] public bool enoughIsEnough;
    [HideInInspector] public bool puttingSandBagOnTheWall;
    public static WallBuilder Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        animator.AnimationState.Event += OnMyEvent;
    }
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        if (animationName.name.Equals(currentAnimation))
            return;
        animator.state.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("hoda"))
        {
            SetAnimation(hoda, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("panika"))
        {
            SetAnimation(panika, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("stoji"))
        {
            SetAnimation(stoji, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("veselje"))
        {
            SetAnimation(veselje, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("vreca_pojaviseNestane"))
        {
            SetAnimation(vreca_pojaviseNestane, false, FloodTimer.Instance.animationSpeed);
            //AddAnimation(stoji, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("vreca_uzmestavi"))
        {
            SetAnimation(vreca_uzmestavi, false, FloodTimer.Instance.animationSpeed);
            //AddAnimation(stoji, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("idle1"))
        {
            SetAnimation(idle1, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(stoji, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("idle2"))
        {
            SetAnimation(idle2, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(stoji, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("zove"))
        {
            SetAnimation(zove, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("vreca_winBaci"))
        {
            SetAnimation(vreca_winBaci, false, FloodTimer.Instance.animationSpeed);
        }
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
    }
    public void AccelerateAnimation()
    {
        animator.state.SetAnimation(0, currentAnimation, true).TimeScale = FloodTimer.Instance.animationSpeed / 0.3f;
        FloodLevel.Instance.wallBuilderSpeechBubble.SetBool("isTriggered", true);
    }
    public void ReturnAnimationToNormalSpeed()
    {
        animator.state.SetAnimation(0, currentAnimation, true).TimeScale = FloodTimer.Instance.animationSpeed;
        FloodLevel.Instance.wallBuilderSpeechBubble.SetBool("isTriggered", false);
    }
    void OnMyEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if(e.Data.Name == "vrecaNestane")
        {
            if(!enoughIsEnough)
            {
                FloodLevel.Instance.currentSandBag.SetActive(true);
            }
            if (FloodLevel.Instance.thirdWaveBuilt)
            {
                enoughIsEnough = true;
            }
            SetCharacterState("stoji");
            Invoke("NotWorkingAgain", 0.1f);
            Invoke("CheckAgainJustToBeSure", 0.2f);
            Invoke("CheckAgainJustToBeSure", 0.25f);
        }
    }
    private void NotWorkingAgain()
    {
        puttingSandBagOnTheWall = false;
        if (FloodTimer.Instance.flood)
        {
            if (FloodLevel.Instance.floodLevel < 4)
            {
                SetCharacterState("panika");
            }
            else
            {
                SetCharacterState("veselje");
            }
        }
    }
    private void CheckAgainJustToBeSure()
    {
        FloodLevel.Instance.CheckIfTeamsSandBagsAreReady();
    }
}
