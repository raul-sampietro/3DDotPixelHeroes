using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float Speed = 15;
    public GameObject sword;
    public GameObject boomerang;

    Animator animator;
    InventoryManager inventory;
    Rigidbody rb;

    Vector3 prevLookDirection = Vector3.forward;

    bool swordInstantiated = false;
    GameObject swordObj;
    Vector3 initialAttackDirection;
    bool swiped = false;

    bool boomerangThrown = false;
    GameObject boomerangObj;

    private Vector2 actualRoomCoordinates, prevRoomCoordinates = new(0,0);
    private Vector2 sizeOfRoom = new(265, 192);

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        inventory = gameObject.GetComponent<InventoryManager>();
        rb = GetComponent<Rigidbody>();
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

        bool shiftPressed;
        if (inventory.HasItem(boomerang.tag))
            shiftPressed = Input.GetKey(KeyCode.LeftShift);
        else shiftPressed = false;

        bool isOnAttackStart = animator.GetCurrentAnimatorStateInfo(0).IsName("AttackStart");
        bool isOnAttackEnd = animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd");
        bool isOnThrowStart = animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowStart");
        bool isOnThrowEnd = animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowEnd");

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

        // Set isAttacking && isThrowing animator input
        if (shiftPressed && attackDirection != Vector3.zero)
        {
            animator.SetBool("isThrowing", true);
        } 
        else
        {
            animator.SetBool("isThrowing", false);

            if (attackDirection != Vector3.zero)
                animator.SetBool("isAttacking", true);
            else
                animator.SetBool("isAttacking", false);
        }

        // Apply the inputs to the player
        if (isOnThrowStart)
        {
            if (!boomerangThrown)
            {
                boomerangThrown = true;
                Vector3 forward = transform.forward * 5;
                Vector3 up = transform.up * 5;
                Vector3 boomerangRelPos = forward + up;
                boomerangObj = Instantiate(boomerang, transform.position + boomerangRelPos, Quaternion.identity);
                boomerangObj.GetComponent<BoomerangAttack>().Initialize(gameObject, transform.position + boomerangRelPos, transform.forward);
            }
        }
        else if (isOnAttackEnd)
        {
            // End of the attack: Retract sword
            if (swordInstantiated)
            {
                swordInstantiated = false;
                swordObj.GetComponent<KnightSwordSpawn>().Disappear();
            }
                
        }
        else if (isOnAttackStart)
        {
            // Start of the attack: Instantiate sword
            if (!swordInstantiated)
            {
                rb.velocity = Vector3.zero; // Stop translating
                swordInstantiated = true;
                swiped = false;
                Vector3 forward = transform.forward * 5;
                Vector3 up = transform.up * 5;
                Vector3 swordRelPos = forward + up;
                swordObj = Instantiate(sword, transform.position + swordRelPos, transform.rotation);
                initialAttackDirection = transform.forward;
            }
            // Already attacking: Allow for 90 degree sword swipe
            else if (!swiped && attackDirection != initialAttackDirection)
            {
                swiped = true;

                // Rotate Player to face the new attackDirection
                Quaternion.FromToRotation(transform.forward, attackDirection)
                    .ToAngleAxis(out float angle, out Vector3 axis);
                if (axis.y < 0.0f) angle = -angle;
                transform.Rotate(new Vector3(0, 1, 0), angle*2, Space.World);

                // Rotate KnightSword
                swordObj.GetComponent<KnightSwordSwipe>().SwipeTo(gameObject, attackDirection);
            }
        }
        // Not attacking at all
        else
        {
            // Rotate to face lookDirection
            Quaternion rotation = Quaternion.FromToRotation(transform.forward, lookDirection);
            rotation.ToAngleAxis(out float angle, out Vector3 axis);
            if (axis.y < 0.0f) angle = -angle;
            transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);

            // Translate
            rb.velocity = Speed * Vector3.Normalize(moveDirection);

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

    public void BoomerangIsBack()
    {
        boomerangThrown = false;
    }
}
