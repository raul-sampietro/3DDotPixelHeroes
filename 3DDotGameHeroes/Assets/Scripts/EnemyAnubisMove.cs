using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisMove : MonoBehaviour
{
    Vector3 movDirection = new(0,0,0);
    private float speed = 15;
    public float maxRotationSpeed = 180.0f;
    private string movementPattern;

    private GameObject knight = null;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 3 is the obstable layer number
        if (collision.gameObject.layer == 3)
        {
            movDirection *= -1;
        }
    }

    public void SetMovementPattern(string movementPatter)
    {
        this.movementPattern = movementPatter;
    }

    private void MoveEnemy()
    {
        animator.SetBool("isShooting", false);
        animator.SetBool("isMoving", true);
        if (movDirection == new Vector3(0, 0, 0))
        {
            if (movementPattern == "Vertically")
                movDirection = Vector3.forward;
            else if (movementPattern == "Horizontally")
                movDirection = Vector3.right;
        }
        // Rotate the enemy to face the movement direction 
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, movDirection);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > maxRotationSpeed * Time.deltaTime) angle = maxRotationSpeed * Time.deltaTime;
        if (axis.y < 0.0f) angle = -angle;
        transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);

        // Move the enemy
        transform.Translate(speed * Time.deltaTime * Vector3.Normalize(movDirection), Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
        {
            knight = GameObject.Find("Knight");
        }


        // Stop moving and prepare to shoot the player
        if (Physics.Linecast(transform.position, knight.transform.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject == knight)
            {
                animator.SetBool("isMoving", false);
                // Locate the direction to reach  the player
                Vector3 direction = knight.transform.position - transform.position;
                direction = Vector3.Normalize(direction);

                // Rotate the enemy to face the player
                Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction);
                rotation.ToAngleAxis(out float angle, out Vector3 axis);
                if (angle > maxRotationSpeed * Time.deltaTime) angle = maxRotationSpeed * Time.deltaTime;
                if (axis.y < 0.0f) angle = -angle;

                transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.forward, hit.normal), 0.8f);
            }
            else
                MoveEnemy();
        }
        else // Move according to the pattern
            MoveEnemy();
    }
}
