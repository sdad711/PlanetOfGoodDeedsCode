using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class VjevericaUI : MonoBehaviour
{
    public SkeletonGraphic animator;
    [SerializeField] private AnimationReferenceAsset blinki, avatar_govori, avatar_hlace, avatar_hlace2, avatar_hlace3, avatar_idle1, avatar_idle2, avatar_kapa, avatar_kapa2, avatar_kapa3, avatar_majica, avatar_majica2, avatar_majica3, avatar_stoji;
    private string currentAnimation;
    [SerializeField] private float blinkingOccurance, blinkingSpeed;
    [SpineSlot] public string tijelo, rukav_d, rukav_lj, kapa, hlace;
    [SpineAttachment] public string bijeli_tijelo, civil_tijelo, crveni_prsluk, crveni_tijelo, bijeli_rukav_d, civil_rukav_d, crveni_rukav_d, bijeli_rukav_lj, civil_rukav_lj, crveni_rukav_lj, bijeli_kapa, crveni_kapa, sivi_kapa, zeleni_kapa, civil_hlace, crni_hlace, crveni_hlace;
    private string[] idleAnimationNamesAvatar;
    private string[] avatarKapaAnimationNames;
    private string[] avatarMajicaAnimationNames;
    private string[] avatarHlaceAnimationNames;
    private bool goForIdleAvatar, timerSet;
    private float timer;
    public static VjevericaUI Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("Blinking", 0, blinkingOccurance);
        idleAnimationNamesAvatar = new string[] { "avatar_idle1", "avatar_idle2" };
        avatarKapaAnimationNames = new string[] { "avatar_kapa", "avatar_kapa2", "avatar_kapa3" };
        avatarHlaceAnimationNames = new string[] { "avatar_hlace", "avatar_hlace2", "avatar_hlace3" };
        avatarMajicaAnimationNames = new string[] { "avatar_majica", "avatar_majica2", "avatar_majica3" };
        SetCharacterState("avatar_stoji");
    }
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        //if (animationName.name.Equals(currentAnimation))
        //    return;
        animator.AnimationState.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
        if (currentAnimation == "avatar_stoji")
        {
            timerSet = true;
            goForIdleAvatar = true;
        }
        else
        {
            timerSet = false;
            goForIdleAvatar = false;
        }
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("avatar_govori"))
        {
            SetAnimation(avatar_govori, true, 1);
        }
        if (state.Equals("avatar_stoji"))
        {
            SetAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_hlace"))
        {
            SetAnimation(avatar_hlace, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_hlace2"))
        {
            SetAnimation(avatar_hlace2, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_hlace3"))
        {
            SetAnimation(avatar_hlace3, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_idle1"))
        {
            SetAnimation(avatar_idle1, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_idle2"))
        {
            SetAnimation(avatar_idle2, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_kapa"))
        {
            SetAnimation(avatar_kapa, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_kapa2"))
        {
            SetAnimation(avatar_kapa2, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_kapa3"))
        {
            SetAnimation(avatar_kapa3, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_majica"))
        {
            SetAnimation(avatar_majica, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_majica2"))
        {
            SetAnimation(avatar_majica2, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
        if (state.Equals("avatar_majica3"))
        {
            SetAnimation(avatar_majica3, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
    }
    private void Blinking()
    {
        Spine.TrackEntry blinkAnimationEntry = animator.AnimationState.AddAnimation(1, blinki, false, 0f);
        Debug.Log("mix Blink");
        blinkAnimationEntry.TimeScale = blinkingSpeed;
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.AnimationState.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
        currentAnimation = animationName.name;
        if (currentAnimation == "avatar_stoji")
        {
            timerSet = true;
            goForIdleAvatar = true;
        }
        else
        {
            timerSet = false;
            goForIdleAvatar = false;
        }
    }
    public void ChangeHat(string itemName)
    {
        ClearAllHats();
        switch (itemName)
        {
            case "bijeli_kapa":
                animator.Skeleton.SetAttachment(kapa, bijeli_kapa);
                break;
            case "crveni_kapa":
                animator.Skeleton.SetAttachment(kapa, crveni_kapa);
                break;
            case "sivi_kapa":
                animator.Skeleton.SetAttachment(kapa, sivi_kapa);
                break;
            case "zeleni_kapa":
                animator.Skeleton.SetAttachment(kapa, zeleni_kapa);
                break;
            default:
                Debug.Log("wrong tag on sprite to attach to Spine skeleton");
                break;
        }
    }
    public void ClearAllHats()
    {
        animator.Skeleton.SetAttachment(kapa, null);
    }
    public void ChangeShirt(string itemName)
    {
        ClearAllShirts();
        switch (itemName)
        {
            case "bijeli_tijelo":
                animator.Skeleton.SetAttachment(tijelo, bijeli_tijelo);
                animator.Skeleton.SetAttachment(rukav_d, bijeli_rukav_d);
                animator.Skeleton.SetAttachment(rukav_lj, bijeli_rukav_lj);
                break;
            case "civil_tijelo":
                animator.Skeleton.SetAttachment(tijelo, civil_tijelo);
                animator.Skeleton.SetAttachment(rukav_d, civil_rukav_d);
                animator.Skeleton.SetAttachment(rukav_lj, civil_rukav_lj);
                break;
            case "crveni_prsluk":
                animator.Skeleton.SetAttachment(tijelo, crveni_prsluk);
                animator.Skeleton.SetAttachment(rukav_d, crveni_rukav_d);
                animator.Skeleton.SetAttachment(rukav_lj, crveni_rukav_lj);
                break;
            case "crveni_tijelo":
                animator.Skeleton.SetAttachment(tijelo, crveni_tijelo);
                animator.Skeleton.SetAttachment(rukav_d, crveni_rukav_d);
                animator.Skeleton.SetAttachment(rukav_lj, crveni_rukav_lj);
                break;
            default:
                Debug.Log("wrong tag on sprite to attach to Spine skeleton");
                break;
        }
    }
    public void ClearAllShirts()
    {
        animator.Skeleton.SetAttachment(tijelo, null);
        animator.Skeleton.SetAttachment(rukav_d, null);
        animator.Skeleton.SetAttachment(rukav_lj, null); 
    }
    public void ChangePants(string itemName)
    {
        ClearAllPants();
        switch (itemName)
        {
            case "civil_hlace":
                animator.Skeleton.SetAttachment(hlace, civil_hlace);
                break;
            case "crni_hlace":
                animator.Skeleton.SetAttachment(hlace, crni_hlace);
                break;
            case "crveni_hlace":
                animator.Skeleton.SetAttachment(hlace, crveni_hlace);
                break;
            default:
                Debug.Log("wrong tag on sprite to attach to Spine skeleton");
                break;
        }
    }
    public void ClearAllPants()
    {
        animator.Skeleton.SetAttachment(hlace, null);
    }
    public void RandomHatAnimation()
    {
        var animation = Random.Range(0, avatarKapaAnimationNames.Length);
        SetCharacterState(avatarKapaAnimationNames[animation]);
    }
    public void RandomPantsAnimation()
    {
        var animation = Random.Range(0, avatarHlaceAnimationNames.Length);
        SetCharacterState(avatarHlaceAnimationNames[animation]);
    }
    public void RandomMajicaAnimation()
    {
        var animation = Random.Range(0, avatarMajicaAnimationNames.Length);
        SetCharacterState(avatarMajicaAnimationNames[animation]);
    }
    private void Update()
    {
        if (goForIdleAvatar)
        {
            if (timerSet)
            {
                timer = Random.Range(4f, 6f);
                timerSet = false;
                Debug.Log("timer set");
            }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                var animation = Random.Range(0, idleAnimationNamesAvatar.Length);
                SetCharacterState(idleAnimationNamesAvatar[animation]);
                timerSet = true;

            }
        }
    }


}

