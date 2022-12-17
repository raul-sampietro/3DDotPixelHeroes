using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float Speed = 15;
    public GameObject sword;

    Animator animator;

    Vector3 prevLookDirection = Vector3.forward;

    bool swordInstantiated = false;
    GameObject swordObj;

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

        // Set isMoving animator input
        if (moveDirection != Vector3.zero) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        // Compose attackDirection
        Vector3 attackDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            attackDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            attackDirection += -Vector3.forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            attackDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            attackDirection += Vector3.right;
        }

        bool isOnAttackStart = animator.GetCurrentAnimatorStateInfo(0).IsName("AttackStart");
        bool isOnAttackEnd = animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd");

        // Set lookDirection
        Vector3 lookDirection;
        if (isOnAttackStart || isOnAttackEnd)
        {
            lookDirection = prevLookDirection;
        }
        else if (attackDirection == Vector3.zero)
        {
            lookDirection = moveDirection;
        }
        else {
            lookDirection = attackDirection;
        }

        // Set isAttacking animator input
        if (attackDirection != Vector3.zero) animator.SetBool("isAttacking", true);
        else animator.SetBool("isAttacking", false);

        // Apply the inputs to the player
        if (isOnAttackEnd)
        {
            if (swordInstantiated)
            {
                swordInstantiated = false;
                Destroy(swordObj);
            }
                
        }
        else if (isOnAttackStart)
        {
            if (!swordInstantiated)
            {
                swordInstantiated = true;
                Vector3 forward = transform.forward * 5;
                Vector3 up = -transform.up * 5;
                Vector3 swordRelPos = forward + up;
                Debug.Log(swordRelPos);
                swordObj = Instantiate(sword, transform.position + swordRelPos, transform.rotation);
                
            }
        }
        else
        {
            // Rotate to face lookDirection
            Quaternion rotation = Quaternion.FromToRotation(transform.forward, lookDirection);
            rotation.ToAngleAxis(out float angle, out Vector3 axis);
            if (axis.y < 0.0f) angle = -angle;
            transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);

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
}
