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

    private Vector2 actualRoomCoordinates, prevRoomCoordinates = new(0,0);
    private Vector2 sizeOfRoom = new(265, 192);

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

        // Update the position of the camera if needed
        actualRoomCoordinates.x = (int)(transform.position.x / sizeOfRoom.x);
        actualRoomCoordinates.y = (int)(transform.position.z / sizeOfRoom.y);

        if (actualRoomCoordinates != prevRoomCoordinates)
        {
            GameObject.Find("OverviewCamera").BroadcastMessage("ChangeCurrentRoom", actualRoomCoordinates);
            prevRoomCoordinates = actualRoomCoordinates;
        }
    }
}
