using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] private float movingTime;
    [SerializeField] private float speed;
    private bool moving;
    private void Start()
    {
        InvokeRepeating("ChangeDirection", 0, movingTime);
    }
    private void ChangeDirection()
    {
        moving = !moving;
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
    }

    private void Update()
    {
        if(moving)
            this.gameObject.transform.position += Vector3.right * Time.deltaTime * speed;
        else if(!moving)
            this.gameObject.transform.position -= Vector3.right * Time.deltaTime * speed;
    }
}
