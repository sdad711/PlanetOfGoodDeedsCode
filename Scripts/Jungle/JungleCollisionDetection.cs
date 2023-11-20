using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleCollisionDetection : MonoBehaviour
{
    [HideInInspector] public bool teamPlasticCollision, teamGlassCollision, teamPaperCollision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "teamPlastic")
            teamPlasticCollision = true;
        else if (collision.gameObject.tag == "teamGlass")
            teamGlassCollision = true;
        else if (collision.gameObject.tag == "teamPaper")
            teamPaperCollision = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "teamPlastic")
            teamPlasticCollision = false;
        else if (collision.gameObject.tag == "teamGlass")
            teamGlassCollision = false;
        else if (collision.gameObject.tag == "teamPaper")
            teamPaperCollision = false;
    }
}
