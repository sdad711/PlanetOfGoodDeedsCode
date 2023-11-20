using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilderCollisionDetection : MonoBehaviour
{
    [HideInInspector] public bool wallBuildercollision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wallBuilder")
            wallBuildercollision = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wallBuilder")
            wallBuildercollision = false;
    }
}
