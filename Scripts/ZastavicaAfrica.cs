using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ZastavicaAfrica : MonoBehaviour
{
    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset pada, stoji_divlja, stoji_mirno;
    private string currentAnimation;
    public static ZastavicaAfrica Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

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
        if (state.Equals("pada"))
        {
            SetAnimation(pada, true, 1);
        }
        if (state.Equals("stoji_divlja"))
        {
            SetAnimation(stoji_divlja, true, 1);
        }
        if (state.Equals("stoji_mirno"))
        {
            SetAnimation(stoji_mirno, true, 1);
        }

    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
        currentAnimation = animationName.name;
    }
}
