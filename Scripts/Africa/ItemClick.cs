using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClick : MonoBehaviour
{
    private bool correctClick;
    private void OnMouseDown()
    {
        if (!AfricaLevel.Instance.tutorial)
        {
            if (AfricaLevel.Instance.noMoreClicks == false)
            {
                correctClick = false;
                foreach (GameObject item in AfricaLevel.Instance.stuffInBox)
                {
                    if (item.tag == this.gameObject.tag && !item.activeSelf)
                    {
                        Vjeverica.Instance.SetCharacterState("correct");
                        Vjeverica.Instance.AttachNewItem(item.tag);
                        correctClick = true;
                        item.SetActive(true);
                        AfricaLevel.Instance.ScorePointAudio();
                        AfricaLevel.Instance.AddScorePoints();
                        AfricaLevel.Instance.CheckIfOkToTransitionToNextBox();
                    }
                }
                if (!correctClick)
                {
                    AfricaLevel.Instance.ReduceLives();
                }
            }
        }
        else if (AfricaLevel.Instance.tutorial)
        {
            correctClick = false;
            for (int i = 0; i < AfricaLevel.Instance.stuffInBox.Count; i++)
            {
                if (AfricaLevel.Instance.stuffInBox[i].tag == this.gameObject.tag && !AfricaLevel.Instance.stuffInBox[i].activeSelf)
                {
                    Vjeverica.Instance.SetCharacterState("correct");
                    Vjeverica.Instance.AttachNewItem(AfricaLevel.Instance.stuffInBox[i].tag);
                    correctClick = true;
                    AfricaLevel.Instance.stuffInBox[i].SetActive(true);
                    AfricaLevel.Instance.ScorePointAudio();
                    AfricaLevel.Instance.AddScorePoints();
                    AfricaLevel.Instance.CloseItemTutorialCoroutine(i);
                    AfricaLevel.Instance.CheckIfOkToTransitionToNextBox();
                }
            }
        }
    }
}