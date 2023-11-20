using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plastic : MonoBehaviour
{
    [SerializeField] private GameObject plasticJunk;
    [SerializeField] private JungleCollisionDetection collisionDetection;
    private bool moving;
    private float startPosX, startPosY;
    public Vector3 resetPostion;
    private bool animationIsAccelerated;
    private void Start()
    {
        resetPostion = this.transform.localPosition;
    }
    private void OnMouseDown()
    {
        if (!JungleLevel.Instance.tutorial || JungleLevel.Instance.plasticTutorial)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.GetComponent<SpriteRenderer>().sortingOrder = 6;
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
        if (!JungleLevel.Instance.tutorial)
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 4;
            moving = false;
            if (animationIsAccelerated)
                ReturnToNormalAnimations();
            if (collisionDetection.teamPlasticCollision && TeamPlastic.Instance.askForJunk && plasticJunk.activeSelf)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                TeamPlastic.Instance.ThrowJunk();
                this.gameObject.SetActive(false);
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
        }
        else if (JungleLevel.Instance.plasticTutorial)
        {
            moving = false;
            if (animationIsAccelerated)
                ReturnToNormalAnimations();
            if (collisionDetection.teamPlasticCollision && TeamPlastic.Instance.askForJunk && plasticJunk.activeSelf)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                TeamPlastic.Instance.ThrowJunkTutorial();
                JungleLevel.Instance.StartPaperTutorial();
                this.gameObject.SetActive(false);
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
        TeamPlastic.Instance.ReturnAnimationToNormalSpeed();
    }
    private void Update()
    {
        if (moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
            if (collisionDetection.teamPlasticCollision && TeamPlastic.Instance.askForJunk && plasticJunk.activeSelf)
            {
                if (!animationIsAccelerated)
                {
                    TeamPlastic.Instance.AccelerateAnimation();
                    animationIsAccelerated = true;
                }
            }
            else
            {
                if (animationIsAccelerated)
                    ReturnToNormalAnimations();
            }
        }
    }
}
