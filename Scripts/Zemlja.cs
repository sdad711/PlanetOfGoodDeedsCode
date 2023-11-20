using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Zemlja : MonoBehaviour
{
    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset faca_gleda_desno, faca_gleda_dolje, faca_gleda_gore, faca_mix_blink, faca_nestaje, faca_pohvala, faca_postaje, faca_stoji, zemlja_prazna;
    private string currentAnimation;
    [SerializeField] private float blinkingOccurance, blinkingSpeed;
    public static Zemlja Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InvokeRepeating("Blinking", 0, blinkingOccurance);
    }
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        //if (animationName.name.Equals(currentAnimation))
        //    return;
        animator.state.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("faca_gleda_desno"))
        {
            SetAnimation(faca_gleda_desno, false, 1);
        }
        if (state.Equals("faca_gleda_dolje"))
        {
            SetAnimation(faca_gleda_dolje, false, 1);
        }
        if (state.Equals("faca_gleda_gore"))
        {
            SetAnimation(faca_gleda_gore, false, 1);
        }
        if (state.Equals("faca_nestaje"))
        {
            SetAnimation(faca_nestaje, false, 1);
            AddAnimation(zemlja_prazna, true, 1);
        }
        if (state.Equals("faca_pohvala"))
        {
            SetAnimation(faca_pohvala, false, 1);
            AddAnimation(faca_stoji, true, 1);
        }
        if (state.Equals("faca_postaje"))
        {
            SetAnimation(faca_postaje, false, 1);
            AddAnimation(faca_stoji, true, 1);
        }
    }
    private void Blinking()
    {
        Spine.TrackEntry blinkAnimationEntry = animator.state.AddAnimation(1, faca_mix_blink, false, 0f);
        blinkAnimationEntry.TimeScale = blinkingSpeed;
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
        currentAnimation = animationName.name;
    }
}
