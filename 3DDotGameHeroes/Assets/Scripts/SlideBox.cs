using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBox : MonoBehaviour
{
    public float speed = 30f;

    private Vector3 from, to;
    bool moving;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moving = false;
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Vector3 playerDirection = Vector3.Normalize(transform.position - collision.gameObject.transform.position);
            playerDirection.y = 0;
            playerDirection = Vector3.Normalize(playerDirection);
            Vector3 dir = Vector3.zero;
            if (Mathf.Abs(playerDirection.z) > Mathf.Abs(playerDirection.x))
            {
                // Vertical movement
                if (playerDirection.z > 0) dir = transform.forward;
                else dir = -transform.forward;
            }
            else 
            {
                // Horizontal movement
                if (playerDirection.x > 0) dir = transform.right;
                else dir = -transform.right;
            }
            rb.velocity = dir * speed;
            moving = true;
        }
    }
}
