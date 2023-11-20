using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Heart02 : MonoBehaviour
{
    public SkeletonGraphic animator;
    [SerializeField] private AnimationReferenceAsset srce_puno, srce_umire, srce_prazno, srce_povratak;
    private string currentAnimation;
    public static Heart02 Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        if (animationName.name.Equals(currentAnimation))
            return;
        animator.AnimationState.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("srce_puno"))
            SetAnimation(srce_puno, true, 1f);
        if (state.Equals("srce_prazno"))
            SetAnimation(srce_prazno, true, 1f);
        if (state.Equals("srce_umire"))
        {
            SetAnimation(srce_umire, false, 1f);
            AddAnimation(0, srce_prazno, true, 1f);
        }
        if (state.Equals("srce_povratak"))
        {
            SetAnimation(srce_povratak, false, 1f);
            AddAnimation(0, srce_puno, true, 1f);
        }
    }
    private void AddAnimation(int track, AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.AnimationState.AddAnimation(track, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
    }
}

