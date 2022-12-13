using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisMove : MonoBehaviour
{

    Vector3 prevDirection = Vector3.forward;
    Vector3 direction = Vector3.forward;
    public float maxRotationSpeed = 180.0f;
    private string movementPattern;

    private GameObject knight = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMovementPattern(string movementPatter)
    {
        this.movementPattern = movementPatter;
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
            knight = GameObject.Find("Knight");

        // Stop moving and shoot the player
        if (!Physics.Linecast(transform.position, knight.transform.position))
        {
            // Locate the direction to reach  the player
            Vector3 direction = knight.transform.position - transform.position;
            direction = Vector3.Normalize(direction);

            // Rotate the enemy to face the player
            Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction);
            rotation.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > maxRotationSpeed * Time.deltaTime) angle = maxRotationSpeed * Time.deltaTime;
            if (axis.y < 0.0f) angle = -angle;
            transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);
        }
        else // Move according to the pattern
        {
            switch (movementPattern)
            {
                case "Vertically":

                    break;

                case "Horizontally":

                    break;

                default:
                    break;
            }
        }
    }
}
