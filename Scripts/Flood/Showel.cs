using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showel : MonoBehaviour
{
    [SerializeField] private GameObject team01Showel, team02Showel, team03Showel, team04Showel, team05Showel;
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
        if (!FloodLevel.Instance.tutorial || FloodLevel.Instance.showelTutorial)
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
            if (team01Showel.activeSelf && Team01.Instance.askForShowel && collisionDetection.team01Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team01.Instance.StartDiggingSand();
            }
            else if (team02Showel.activeSelf && Team02.Instance.askForShowel && collisionDetection.team02Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team02.Instance.StartDiggingSand();
            }
            else if (team03Showel.activeSelf && Team03.Instance.askForShowel && collisionDetection.team03Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team03.Instance.StartDiggingSand();
            }
            else if (team04Showel.activeSelf && Team04.Instance.askForShowel && collisionDetection.team04Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team04.Instance.StartDiggingSand();
            }
            else if (team05Showel.activeSelf && Team05.Instance.askForShowel && collisionDetection.team05Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                Team05.Instance.StartDiggingSand();
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
        }
        else if (FloodLevel.Instance.showelTutorial)
        {
            moving = false;
            if (animationIsAccelerated)
                ReturnToNormalAnimations();
            if (team01Showel.activeSelf && collisionDetection.team01Collision)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                FloodLevel.Instance.StartBagTutorial();
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
        }
    }
    private void ReturnToNormalAnimations()
    {
        Debug.Log("return to normal animation speed");
        animationIsAccelerated = false;
        Team01.Instance.ReturnAnimationToNormalSpeed();
        Team02.Instance.ReturnAnimationToNormalSpeed();
        Team03.Instance.ReturnAnimationToNormalSpeed();
        Team04.Instance.ReturnAnimationToNormalSpeed();
        Team05.Instance.ReturnAnimationToNormalSpeed();
    }
    private void Update()
    {
        if(moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
            if (team01Showel.activeSelf && Team01.Instance.askForShowel && collisionDetection.team01Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team01.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team02Showel.activeSelf && Team02.Instance.askForShowel && collisionDetection.team02Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team02.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team03Showel.activeSelf && Team03.Instance.askForShowel && collisionDetection.team03Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team03.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team04Showel.activeSelf && Team04.Instance.askForShowel && collisionDetection.team04Collision)
            {
                if (!animationIsAccelerated)
                {
                    Team04.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else if (team05Showel.activeSelf && Team05.Instance.askForShowel && collisionDetection.team05Collision)
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
