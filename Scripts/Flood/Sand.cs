using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Sand : MonoBehaviour
{
    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset prazno, puno, triFaze, triFaze_unatrag;
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
        if (state.Equals("prazno"))
        {
            SetAnimation(prazno, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("puno"))
        {
            SetAnimation(puno, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("triFaze"))
        {
            SetAnimation(triFaze, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(puno, true, FloodTimer.Instance.animationSpeed);
        }
        if (state.Equals("triFaze_unatrag"))
        {
            SetAnimation(triFaze_unatrag, false, FloodTimer.Instance.animationSpeed);
            AddAnimation(prazno, true, FloodTimer.Instance.animationSpeed);
        }
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
    }
}
