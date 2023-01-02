using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubis : Enemy
{
    public int shootingOffset;
    public GameObject shot;
    
    private int iniShootingOffset;
    private bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        iniShootingOffset = shootingOffset;
        GetAnimator();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 3 is the obstable layer number
        if (collision.gameObject.layer == 3)
        {
            movDirection *= -1;
        }
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
        RotateYAxes(movDirection);

        // Move the enemy
        transform.Translate(movSpeed * Time.deltaTime * Vector3.Normalize(movDirection), Space.World);
    }

    private void ShootPlayer()
    {
        animator.SetBool("isMoving", false);

        // Locate the direction to reach  the player
        Vector3 direction = knight.transform.position - transform.position;
        direction = Vector3.Normalize(direction);

        // Rotate the enemy to face the player
        RotateYAxes(direction);

        Debug.Log(shootingOffset);
        Debug.Log(isShooting);
        // Check if its time to shoot
        if (shootingOffset < 0)
        {
            shootingOffset = iniShootingOffset;
            isShooting = false;
            animator.SetBool("isShooting", false);

            // Create the shoot
            Instantiate(shot, transform.position + transform.forward + new Vector3(0.0f, 5.0f, 0.0f), transform.rotation);
        }
        else if (shootingOffset < 50 && !isShooting)
        {
            animator.SetBool("isShooting", true);
            isShooting = true;
        }
        else
        {
            shootingOffset -= 1;
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

        // If the player is visible shoot him, otherwise keep moving
        if (Physics.Linecast(transform.position, knight.transform.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject == knight)
            {
                ShootPlayer();
            }
            else
                MoveEnemy();
        }
        else // Move according to the pattern
            MoveEnemy();
    }
}
