using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float offsety;
    private float offsetX;
    [SerializeField] private float offsetXPositive, offsetXNegative;
    private void Update()
    {
        if (target.transform.position.x > 0)
        {
            if (offsetX == offsetXPositive)
            {
            }
            else
            {
                offsetX = offsetXPositive;
                this.gameObject.transform.Rotate(0, 180, 0);
            }

        }
        else if (target.transform.position.x <= 0)
        {
            if (offsetX == offsetXNegative)
            {
            }
            else
            {
                offsetX = offsetXNegative;
                this.gameObject.transform.Rotate(0, 180, 0);
            }
        }
        this.gameObject.transform.position = new Vector3(target.gameObject.transform.position.x + offsetX, target.gameObject.transform.position.y + offsety, target.transform.position.z);
    }
}
