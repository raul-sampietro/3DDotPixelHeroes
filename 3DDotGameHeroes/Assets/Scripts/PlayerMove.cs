using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float Speed = 15;

    Animator animator;

    Vector3 prevLookDirection = Vector3.forward;
    Vector3 lookDirection = Vector3.forward;
    Vector3 moveDirection;

    int roomNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Compose moveDirection
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        // Set animation
        if (moveDirection != Vector3.zero) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        // Compose lookDirection
        Vector3 lookDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            lookDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            lookDirection += -Vector3.forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lookDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            lookDirection += Vector3.right;
        }

        // Set lookDirection
        if (lookDirection == Vector3.zero) lookDirection = moveDirection;

        // Rotate to face lookDirection
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.FromToRotation(transform.forward, lookDirection);
            rotation.ToAngleAxis(out float angle, out Vector3 axis);
            if (axis.y < 0.0f) angle = -angle;
            transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);
        }
        else lookDirection = prevLookDirection;

        // Translate
        transform.Translate(Speed * Time.deltaTime * Vector3.Normalize(moveDirection), Space.World);
        prevLookDirection = lookDirection;


        // Check if the room has been changed TODO change value to check use result of division x/240
        if (transform.position.x > 240)
        {
            if (roomNumber == 0)
            {
                roomNumber += 1;
                GameObject.Find("OverviewCamera").BroadcastMessage("ChangeCurrentRoom", roomNumber);
            }
        }
        else if (transform.position.x > 480)
        {
            if (roomNumber == 1)
            {
                roomNumber += 1;
                GameObject.Find("OverviewCamera").BroadcastMessage("ChangeCurrentRoom", roomNumber);
            }
        }
        else if (transform.position.x < 240)
        {
            if (roomNumber == 1)
            {
                roomNumber -= 1;
                GameObject.Find("OverviewCamera").BroadcastMessage("ChangeCurrentRoom", roomNumber);
            }
        }

    }
}
