using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinalBoss : Enemy
{
    private bool attackInProgress = false;
    private Vector3 attackDirection;

    private int coolDownIni = 250;
    private int coolDown;

    // Start is called before the first frame update
    void Start()
    {
        GetAnimator();
        maxRotationSpeed = 300.0f;
        damageMatrix = DamageMatrix.Instance;
        coolDown = coolDownIni;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 2)
            Debug.Log("Collision1 " + collision.gameObject.name);
        // 3 is the obstable layer number
        if (collision.gameObject.layer == 3)
        {
            movDirection = collision.gameObject.transform.forward;
            attackInProgress = false;
            coolDown = coolDownIni;
        }
        else if (collision.gameObject.CompareTag("KnightSword") || collision.gameObject.CompareTag("Boomerang"))
        {
            int damage = damageMatrix.DoesDamage(gameObject.tag, collision.gameObject.tag);
            if (damage > 0)
                collision.gameObject.GetComponent<HealthSystem>().Damage(damage);
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", true);

        // Rotate the enemy to face the attacking direction
        if (attackDirection != null)
            RotateYAxes(attackDirection);

        // Initialise the attack
        if (!attackInProgress)
        {
            // Locate the initial direction to reach the player
            attackDirection = knight.transform.position - transform.position;
            attackDirection = Vector3.Normalize(attackDirection);
        }
        // Continue with the attack
        else
        {
            transform.Translate(movSpeed * 2 * Time.deltaTime * attackDirection, Space.World);
        }
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
                attackInProgress = true;
            }
            else
            {
                if (coolDown > -1)
                    --coolDown;
                attackInProgress = false;
                MoveEnemy();
            }
            
            // Check that the model does not cross the obstacles
            if (Physics.Linecast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitObstacle))
                if (hitObstacle.collider.gameObject.layer == 3 && hitObstacle.distance < 50)
                {
                    attackInProgress = false;
                    MoveEnemy();
                }
            
        }
        else // Move according to the pattern
        {
            attackInProgress = false;
            MoveEnemy();
        }
    }
}
