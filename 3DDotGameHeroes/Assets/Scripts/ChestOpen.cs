using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 playerDirection = Vector3.Normalize(collision.gameObject.transform.position - transform.position);
        float angle = Vector3.Angle(playerDirection, transform.forward);
        if (collision.gameObject.layer == 7 && angle < 35)
            animator.SetBool("isOpening", true);
    }
}
