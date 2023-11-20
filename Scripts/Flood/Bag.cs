using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] private GameObject team01Bag, team02Bag, team03Bag, team04Bag, team05Bag;
    [SerializeField] private CollisionDetection collisionDetection;
    private bool moving;
    private float startPosX, startPosY;
    private Vector3 resetPostion;
    private bool animationIsAccelerated;
    private void Start()
    {
        resetPostion = this.transform.localPosition;
    }
    private void OnMouseDown()
    {
        if (!FloodLevel.Instance.tutorial || FloodLevel.Instance.bagTutorial)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("mouse down " + this.gameObject.name);
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - this.transform.localPosition.x;
                startPosX = mousePos.y - this.transform.localPosition.y;

                moving = true;
            }
        }
    
    }
    private void OnMouseUp()
    {
        if (!FloodLevel.Instance.tutorial)
        {
            moving = false;
            if(animationIsAccelerated)
                ReturnToNormalAnimations();
            if (team01Bag.activeSelf && Team01.Instance.askForBag && collisionDetection.team01Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team01.Instance.StartPuttingSandInBag();
            }
            else if (team02Bag.activeSelf && Team02.Instance.askForBag && collisionDetection.team02Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team02.Instance.StartPuttingSandInBag();
            }
            else if (team03Bag.activeSelf && Team03.Instance.askForBag && collisionDetection.team03Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team03.Instance.StartPuttingSandInBag();
            }
            else if (team04Bag.activeSelf && Team04.Instance.askForBag && collisionDetection.team04Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team04.Instance.StartPuttingSandInBag();
            }
            else if (team05Bag.activeSelf && Team05.Instance.askForBag && collisionDetection.team05Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team05.Instance.StartPuttingSandInBag();
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
        }
        else if (FloodLevel.Instance.bagTutorial)
        {
            moving = false;
            if (animationIsAccelerated)
                ReturnToNormalAnimations();
            if (team03Bag.activeSelf && collisionDetection.team03Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                FloodLevel.Instance.StartRopeTutorial();
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
        }

    }
    private void ReturnToNormalAnimations()
    {
        animationIsAccelerated = false;
        Team01.Instance.ReturnAnimationToNormalSpeed();
        Team02.Instance.ReturnAnimationToNormalSpeed();
        Team03.Instance.ReturnAnimationToNormalSpeed();
        Team04.Instance.ReturnAnimationToNormalSpeed();
        Team05.Instance.ReturnAnimationToNormalSpeed();
    }
    private void Update()
    {
        if (moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
            if (team01Bag.activeSelf && Team01.Instance.askForBag && collisionDetection.team01Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team01.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team02Bag.activeSelf && Team02.Instance.askForBag && collisionDetection.team02Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team02.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team03Bag.activeSelf && Team03.Instance.askForBag && collisionDetection.team03Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team03.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team04Bag.activeSelf && Team04.Instance.askForBag && collisionDetection.team04Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team04.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team05Bag.activeSelf && Team05.Instance.askForBag && collisionDetection.team05Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team05.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else
            {
                if (animationIsAccelerated)
                    ReturnToNormalAnimations();
            }
            if (FloodTimer.Instance.flood)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                moving = false;
            }
        }
    }
}
