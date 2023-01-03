using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : Enemy
{
    private Vector3 attackDirection;

    // Start is called before the first frame update
    void Start()
    {
        GetAnimator();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 3 is the obstable layer number
        if (collision.gameObject.layer == 3)
        {
            movDirection *= -1;
        }
        //DestroyWithParticles();
    }

    private void AttackPlayer()
    {
        animator.SetBool("isAttacking", true);

        // Set the direction
        attackDirection = knight.transform.position - transform.position;
        attackDirection = Vector3.Normalize(attackDirection);

        // Rotate the enemy to face the attacking direction
        RotateYAxes(attackDirection);
       
        transform.Translate(movSpeed * 1.5f * Time.deltaTime * attackDirection, Space.World);
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
            if (hit.collider.gameObject == knight && hit.distance < 40)
            {
                AttackPlayer();
            }
            else
            {
                MoveEnemy();
            }
        }
        else // Move according to the pattern
        {
            MoveEnemy();
        }
    }
}
