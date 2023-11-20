using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class WorldMarker : MonoBehaviour
{

    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset full_blinka, justCircle;
    private string currentAnimation;
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        if (animationName.name.Equals(currentAnimation))
            return;
        animator.state.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("full_blinka"))
            SetAnimation(full_blinka, true, 1f);
        if (state.Equals("justCircle"))
            SetAnimation(justCircle, true, 1f);

    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
    }
    public void BlinkMotherfucker()
    {
        SetCharacterState("full_blinka");
    }
    public void StopBlinkingMotherfucker()
    {
        SetCharacterState("justCircle");
    }

}


