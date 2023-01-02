using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        GetAnimator();
    }

    private void AttackPlayer()
    {
        animator.SetBool("isMoving", false);

        // Locate the direction to reach  the player
        Vector3 direction = knight.transform.position - transform.position;
        direction = Vector3.Normalize(direction);

        // Rotate the enemy to face the player
        RotateYAxes(direction);

        // Proced to attack
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
        {
            knight = FindPlayer();
        }

        // If the player is visible attack him, otherwise keep moving
        if (Physics.Linecast(transform.position, knight.transform.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject == knight)
            {
                AttackPlayer();
            }
            else
                MoveEnemy();
        }
        else // Move according to the pattern
            MoveEnemy();
    }
}
