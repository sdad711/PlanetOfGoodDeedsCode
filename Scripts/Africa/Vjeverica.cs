using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Vjeverica : MonoBehaviour
{
    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset correct, stand, wrong, close_go, blinki, idle1, idle2, mix_item, avatar_govori, avatar_hlace, avatar_idle1, avatar_idle2, avatar_kapa, avatar_majica, avatar_stoji;
    private string currentAnimation;
    [SerializeField] private float blinkingOccurance, blinkingSpeed;
    [SpineSlot] public string flourSlot, jamSlot, sugarSlot, pastaSlot, saltSlot, waterSlot, canSlot, oilSlot;
    [SpineAttachment] public string flourKey, jamKey, sugarKey, pastaKey, saltKey, waterKey, canKey, oilKey;
    [SpineSlot] public string tijelo, rukav_d, rukav_lj, kapa, hlace;
    [SpineAttachment] public string bijeli_tijelo, civil_tijelo, crveni_prsluk, crveni_tijelo, bijeli_rukav_d, civil_rukav_d, crveni_rukav_d, bijeli_rukav_lj, civil_rukav_lj, crveni_rukav_lj, bijeli_kapa, crveni_kapa, sivi_kapa, zeleni_kapa, civil_hlace, crni_hlace, crveni_hlace;
    private string[] idleAnimationNamesAvatar;
    private string[] idleAnimationNames;
    private bool goForIdleAvatar, goForIdleAfrica, timerSet;
    private float timer;   
    [SerializeField] private bool notAvatar;
    public static Vjeverica Instance { get; private set; }

    


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (notAvatar)
        {
            SetCharacterState("close_go");
        }
        var hatName = PlayerPrefs.GetString("HatName");
        var shirtName = PlayerPrefs.GetString("ShirtName");
        var pantsName = PlayerPrefs.GetString("PantsName");
        ChangeHat(hatName);
        ChangeShirt(shirtName);
        ChangePants(pantsName);
        animator.AnimationState.Event += OnMyEvent;
        InvokeRepeating("Blinking", 0, blinkingOccurance);
        idleAnimationNamesAvatar = new string[] { "avatar_idle1", "avatar_idle2" };
        idleAnimationNames = new string[] { "idle1", "idle2" };
    }
    private void SetAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        //if (animationName.name.Equals(currentAnimation))
        //    return;
        animator.state.SetAnimation(0, animationName, loop).TimeScale = timeScale;
        currentAnimation = animationName.name;
        if(currentAnimation == "avatar_stoji")
        {
            timerSet = true;
            goForIdleAvatar = true;
        }
        else if(currentAnimation == "afrika_stoji_afrika")
        { 
            timerSet = true;
            goForIdleAfrica = true;
        }
        else
        {
            timerSet = false;
            goForIdleAvatar = false;
            goForIdleAfrica = false;
        }
        Debug.Log(currentAnimation);
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("correct"))
        {
            SetAnimation(correct, false, AfricaTimer.Instance.animationSpeed);
            MixItem();
            AddAnimation(stand, true, AfricaTimer.Instance.animationSpeed);
        }
        if (state.Equals("afrika_stoji_afrika"))
        {
            SetAnimation(stand, true, AfricaTimer.Instance.animationSpeed);
        }
        if (state.Equals("wrong"))
        {
            SetAnimation(wrong, false, AfricaTimer.Instance.animationSpeed);
            AddAnimation(stand, true, AfricaTimer.Instance.animationSpeed);
        }
        if (state.Equals("close_go"))
        {
            SetAnimation(close_go, false, AfricaTimer.Instance.animationSpeed);
            AddAnimation(stand, true, AfricaTimer.Instance.animationSpeed);
        }
        if (state.Equals("idle1"))
        {
            SetAnimation(idle1, false, AfricaTimer.Instance.animationSpeed);
            AddAnimation(stand, true, AfricaTimer.Instance.animationSpeed);
        }
        if (state.Equals("idle2"))
        {
            SetAnimation(idle2, false, AfricaTimer.Instance.animationSpeed);
            AddAnimation(stand, true, AfricaTimer.Instance.animationSpeed);
        }
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
        if (state.Equals("avatar_majica"))
        {
            SetAnimation(avatar_majica, false, 1);
            AddAnimation(avatar_stoji, true, 1);
        }
    }
    private void Blinking()
    {
        Spine.TrackEntry blinkAnimationEntry = animator.state.AddAnimation(1, blinki, false, 0f);
        blinkAnimationEntry.TimeScale = blinkingSpeed;
    }
    private void MixItem()
    {
        Spine.TrackEntry blinkAnimationEntry = animator.state.AddAnimation(2, mix_item, false, 0f);
        blinkAnimationEntry.TimeScale = 1;
    }
    private void AddAnimation(AnimationReferenceAsset animationName, bool loop, float timeScale)
    {
        Spine.TrackEntry animationEntry = animator.state.AddAnimation(0, animationName, loop, 0f);
        animationEntry.TimeScale = timeScale;
        currentAnimation = animationName.name;
        if (currentAnimation == "avatar_stoji")
        {
            timerSet = true;
            goForIdleAvatar = true;
        }
        else if (currentAnimation == "afrika_stoji_afrika")
        {
            timerSet = true;
            goForIdleAfrica = true;
        }
        else
        {
            timerSet = false;
            goForIdleAvatar = false;
            goForIdleAfrica = false;
        }
        Debug.Log(currentAnimation);
    }
    void OnMyEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "newbox")
        {
            AfricaLevel.Instance.ShowItemsBackGround();
            AfricaLevel.Instance.PlayMusic();
            Invoke("ShowIconsOnScreen", 0.5f);
            SetAttachmentsOnNewBox();
            if(AfricaLevel.Instance.tutorial)
            {
                AfricaLevel.Instance.TutorialHandsPlacement();
            }
        }
    }
    public void SetAttachmentsOnNewBox()
    {
        foreach (GameObject item in AfricaLevel.Instance.stuffInBox)
        {
            if(item.activeSelf)
            {
                var itemTag = item.tag;
                switch (itemTag)
                {
                    case "flour":
                        animator.skeleton.SetAttachment(flourSlot, flourKey);
                        break;
                    case "jam":
                        animator.skeleton.SetAttachment(jamSlot, jamKey);
                        break;
                    case "sugar":
                        animator.skeleton.SetAttachment(sugarSlot, sugarKey);
                        break;
                    case "pasta":
                        animator.skeleton.SetAttachment(pastaSlot, pastaKey);
                        break;
                    case "salt":
                        animator.skeleton.SetAttachment(saltSlot, saltKey);
                        break;
                    case "water":
                        animator.skeleton.SetAttachment(waterSlot, waterKey);
                        break;
                    case "can":
                        animator.skeleton.SetAttachment(canSlot, canKey);
                        break;
                    case "oil":
                        animator.skeleton.SetAttachment(oilSlot, oilKey);
                        break;
                    default:
                        Debug.Log("wrong tag on sprite to attach to Spine skeleton");
                        break;
                }
            }
        }
    }
    public void AttachNewItem(string tag)
    {
        switch (tag)
        {
            case "flour":
                animator.skeleton.SetAttachment(flourSlot, flourKey);
                break;
            case "jam":
                animator.skeleton.SetAttachment(jamSlot, jamKey);
                break;
            case "sugar":
                animator.skeleton.SetAttachment(sugarSlot, sugarKey);
                break;
            case "pasta":
                animator.skeleton.SetAttachment(pastaSlot, pastaKey);
                break;
            case "salt":
                animator.skeleton.SetAttachment(saltSlot, saltKey);
                break;
            case "water":
                animator.skeleton.SetAttachment(waterSlot, waterKey);
                break;
            case "can":
                animator.skeleton.SetAttachment(canSlot, canKey);
                break;
            case "oil":
                animator.skeleton.SetAttachment(oilSlot, oilKey);
                break;
            default:
                Debug.Log("wrong tag on sprite to attach to Spine skeleton");
                break;
        }
    }
    public void CloseAllAttachments()
    {
        animator.skeleton.SetAttachment(flourSlot, null);
        animator.skeleton.SetAttachment(jamSlot, null);
        animator.skeleton.SetAttachment(sugarSlot, null);
        animator.skeleton.SetAttachment(pastaSlot, null);
        animator.skeleton.SetAttachment(saltSlot, null);
        animator.skeleton.SetAttachment(waterSlot, null);
        animator.skeleton.SetAttachment(canSlot, null);
        animator.skeleton.SetAttachment(oilSlot, null);
    }
    private void ChangeHat(string itemName)
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
    private void ClearAllHats()
    {
        animator.Skeleton.SetAttachment(kapa, null);
    }
    private void ChangeShirt(string itemName)
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
    private void ClearAllShirts()
    {
        animator.Skeleton.SetAttachment(tijelo, null);
        animator.Skeleton.SetAttachment(rukav_d, null);
        animator.Skeleton.SetAttachment(rukav_lj, null);
    }
    private void ChangePants(string itemName)
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
    private void ClearAllPants()
    {
        animator.Skeleton.SetAttachment(hlace, null);
    }
    private void ShowIconsOnScreen()
    {
        AfricaLevel.Instance.ShowIconsOnScreen();
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
        if (goForIdleAfrica)
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
                var animation = Random.Range(0, idleAnimationNames.Length);
                SetCharacterState(idleAnimationNames[animation]);
                timerSet = true;

            }
        }
    }


}
