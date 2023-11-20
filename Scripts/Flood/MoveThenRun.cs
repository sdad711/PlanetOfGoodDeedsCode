using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class MoveThenRun : MonoBehaviour
{
    public SkeletonAnimation animator;
    [SerializeField] private AnimationReferenceAsset bjezi, hoda_normalno, iznenadi_se;
    [SerializeField] private float movingTime;
    [SerializeField] private float speed;
    private bool moving, run;
    private void Start()
    {
        moving = true;
        StartCoroutine(Moving());
        animator.state.SetAnimation(0, hoda_normalno, true).TimeScale = speed;
    }
    IEnumerator Moving()
    {
        yield return new WaitForSeconds(movingTime);
        moving = false;
        animator.state.SetAnimation(0, iznenadi_se, false);
        yield return new WaitForSeconds(0.5f);
        animator.state.SetAnimation(0, bjezi, true).TimeScale = speed;
        animator.gameObject.transform.localScale = new Vector3(animator.gameObject.transform.localScale.x * -1, animator.gameObject.transform.localScale.y, animator.gameObject.transform.localScale.z);
        this.gameObject.transform.position -= Vector3.right;
        run = true;
    }

    private void Update()
    {
        if (moving)
            this.gameObject.transform.position += Vector3.right * Time.deltaTime * speed;
        if (run)
            this.gameObject.transform.position -= Vector3.right * Time.deltaTime * speed * 3;
    }
}
