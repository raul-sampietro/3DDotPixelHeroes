using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Animator animator;
    bool opening;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        opening = false;
    }

    public void Open()
    {
        if (!opening)
        {
            animator.SetBool("isOpening", true);
            GetComponent<BoxCollider>().enabled = false;
            opening = true;
        }
        else Debug.LogWarning("Door already opened");
    }

    public bool IsOpened()
    {
        return opening;
    }
}
