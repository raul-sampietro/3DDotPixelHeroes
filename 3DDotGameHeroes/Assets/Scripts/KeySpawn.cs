using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawn : MonoBehaviour
{
    public int framesToAppear = 30;
    public int finalHeight = 10;

    Animator animator;
    float scaleRate, translateRate;
    bool finishedScale, finishedTranslate;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        transform.localScale = new Vector3(0, 0, 0);
        scaleRate = 1f / (float)framesToAppear;
        translateRate = (float)finalHeight / (float)framesToAppear;
        finishedScale = finishedTranslate = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale
        if (transform.localScale.x < 1f)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleRate, transform.localScale.y + scaleRate, transform.localScale.z + scaleRate);
        }
        else if (!finishedScale)
        {
            finishedScale = true;
        }
        //Translate
        if (transform.position.y < finalHeight)
        {
            transform.Translate(0, translateRate, 0);
        }
        else if (!finishedTranslate)
        {
            finishedTranslate = true;
        }
        //Rotate
        if (!finishedScale && !finishedTranslate && !animator.GetBool("isTurning"))
        {
            animator.SetBool("isTurning", true);
        }
        else if (finishedScale && finishedTranslate)
        {
            animator.SetBool("isTurning", false);
        }
    }
}
