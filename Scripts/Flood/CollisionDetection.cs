using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [HideInInspector] public bool team01Collision, team02Collision, team03Collision, team04Collision, team05Collision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "team01")
            team01Collision = true;
        else if (collision.gameObject.tag == "team02")
            team02Collision = true;
        else if (collision.gameObject.tag == "team03")
            team03Collision = true;
        else if (collision.gameObject.tag == "team04")
            team04Collision = true;
        else if (collision.gameObject.tag == "team05")
            team05Collision = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "team01")
            team01Collision = false;
        else if (collision.gameObject.tag == "team02")
            team02Collision = false;
        else if (collision.gameObject.tag == "team03")
            team03Collision = false;
        else if (collision.gameObject.tag == "team04")
            team04Collision = false;
        else if (collision.gameObject.tag == "team05")
            team05Collision = false;
    }
}
