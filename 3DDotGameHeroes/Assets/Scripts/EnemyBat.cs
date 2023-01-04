using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : Enemy
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
        animator.SetBool("isAttacking", true);

        // Set the direction
        attackDirection = knight.transform.position - transform.position;
        attackDirection = Vector3.Normalize(attackDirection);
        attackDirection = new Vector3(attackDirection.x, 0, attackDirection.z);

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
            if (hit.collider.gameObject == knight && hit.distance < 40 && coolDown < 0)
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
