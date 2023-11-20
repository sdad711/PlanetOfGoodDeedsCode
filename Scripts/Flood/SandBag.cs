using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour
{
    [SerializeField] private GameObject sandBag;
    [SerializeField] private WallBuilderCollisionDetection collisionDetection;
    private bool moving;
    private float startPosX, startPosY;
    private Vector3 resetPostion;
    private bool animationIsAccelerated;
    private void Start()
    {
        resetPostion = this.transform.localPosition;
        collisionDetection = GetComponent<WallBuilderCollisionDetection>();
    }
    private void OnMouseDown()
    {
        if (!FloodLevel.Instance.tutorial || FloodLevel.Instance.sandBagTutorial)
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
            if (animationIsAccelerated)
                ReturnToNormalAnimations();
            if (collisionDetection.wallBuildercollision && sandBag.GetComponent<SpriteRenderer>().enabled == true)
            {
                if (this.gameObject.tag == "team01")
                {
                    Team01.Instance.Finished();
                    FloodLevel.Instance.MoveBuilderToBag();
                    FloodLevel.Instance.AddScorePoints();
                    WallBuilder.Instance.puttingSandBagOnTheWall = true;
                }
                else if (this.gameObject.tag == "team02")
                {
                    Team02.Instance.Finished();
                    FloodLevel.Instance.MoveBuilderToBag();
                    FloodLevel.Instance.AddScorePoints();
                    WallBuilder.Instance.puttingSandBagOnTheWall = true;
                }
                else if (this.gameObject.tag == "team03")
                {
                    Team03.Instance.Finished();
                    FloodLevel.Instance.MoveBuilderToBag();
                    FloodLevel.Instance.AddScorePoints();
                    WallBuilder.Instance.puttingSandBagOnTheWall = true;
                }
                else if (this.gameObject.tag == "team04")
                {
                    Team04.Instance.Finished();
                    FloodLevel.Instance.MoveBuilderToBag();
                    FloodLevel.Instance.AddScorePoints();
                    WallBuilder.Instance.puttingSandBagOnTheWall = true;
                }
                else if (this.gameObject.tag == "team05")
                {
                    Team05.Instance.Finished();
                    FloodLevel.Instance.MoveBuilderToBag();
                    FloodLevel.Instance.AddScorePoints();
                    WallBuilder.Instance.puttingSandBagOnTheWall = true;
                }
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
            }
        }
        else if (FloodLevel.Instance.sandBagTutorial)
        {
            moving = false;
            if (animationIsAccelerated)
                ReturnToNormalAnimations();
            if (collisionDetection.wallBuildercollision && sandBag.GetComponent<SpriteRenderer>().enabled == true)
            {
                this.transform.localPosition = new Vector3(resetPostion.x, resetPostion.y, resetPostion.z);
                FloodLevel.Instance.MoveBuilderToBag();
                FloodLevel.Instance.AddScorePoints();
                FloodLevel.Instance.EndingTutorial();
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
        WallBuilder.Instance.ReturnAnimationToNormalSpeed();
    }
    private void Update()
    {
        if (moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
            if (collisionDetection.wallBuildercollision && sandBag.activeSelf)
            {
                if (!animationIsAccelerated)
                {
                    WallBuilder.Instance.AccelerateAnimation();
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
