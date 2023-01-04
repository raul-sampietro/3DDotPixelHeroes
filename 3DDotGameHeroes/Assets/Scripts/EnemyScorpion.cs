using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyScorpion : Enemy
{
    private Vector3 attackDirection;
    private int coolDownIni = 250;
    private int coolDown;

    // Start is called before the first frame update
    void Start()
    {
        GetAnimator();
        coolDown = 0;
        maxRotationSpeed = 400.0f;
        damageMatrix = DamageMatrix.Instance;
    }

    private void AttackPlayer()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", true);

        // Set the direction
        attackDirection = knight.transform.position - transform.position;
        attackDirection = Vector3.Normalize(attackDirection);
        attackDirection = new Vector3(attackDirection.x, 0, attackDirection.z);

        
        if (Math.Round(attackDirection.x,2) > 0.00f)
        {
            attackDirection = Vector3.right;
        }
        else if (Math.Round(attackDirection.x, 2) < 0.00f)
        {
            attackDirection = Vector3.left;
        }
        else if (Math.Round(attackDirection.z, 2) > 0.00f)
        {
            attackDirection = Vector3.forward;
        }
        else if (Math.Round(attackDirection.z, 2) < 0.00f)
        {
            attackDirection = Vector3.back;
        }

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
            if (hit.collider.gameObject == knight && coolDown < 0)
            {
                if (hit.distance < 10) coolDown = coolDownIni;
                AttackPlayer();
            }
            else
            {
                if (coolDown > -1)
                    --coolDown;
                MoveEnemy();
            }
        }
        else // Move according to the pattern
        {
            MoveEnemy();
        }
    }
}
