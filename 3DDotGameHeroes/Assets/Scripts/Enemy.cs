using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movSpeed;
    public string playerTag;

    protected Vector3 movDirection = new(0, 0, 0);
    protected float maxRotationSpeed = 180.0f;
    protected string movementPattern;

    protected GameObject knight = null;
    protected Animator animator;

    public void SetMovementPattern(string movementPatter)
    {
        this.movementPattern = movementPatter;
    }

    public void GetAnimator()
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

    protected GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag(playerTag);
    }

    protected void RotateYAxes(Vector3 direction)
    {
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > maxRotationSpeed * Time.deltaTime) angle = maxRotationSpeed * Time.deltaTime;
        if (axis.y < 0.0f) angle = -angle;
        transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);
    }

    protected void MoveEnemy()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", true);
        if (movDirection == new Vector3(0, 0, 0))
        {
            if (movementPattern == "Vertically")
                movDirection = Vector3.forward;
            else if (movementPattern == "Horizontally")
                movDirection = Vector3.right;
            else if (movementPattern == "Random")
            {
                int randomNumber = Random.Range(0, 40);
                // Randomly change direction
                if (randomNumber == 0)
                {
                    // Randomly choose direction
                    if (randomNumber > 30)
                        movDirection = Vector3.left;
                    else if (randomNumber > 20)
                        movDirection = Vector3.right;
                    else if (randomNumber > 10)
                        movDirection = Vector3.forward;
                    else
                        movDirection = Vector3.back;

                }
            }
        }
        // Rotate the enemy to face the movement direction 
        RotateYAxes(movDirection);

        Debug.Log(movDirection);

        // Move the enemy
        transform.Translate(movSpeed * Time.deltaTime * Vector3.Normalize(movDirection), Space.World);
    }
}
