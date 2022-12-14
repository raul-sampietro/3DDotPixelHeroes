using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisMove : MonoBehaviour
{
    Vector3 direction = new(0,0,0);
    private float speed = 15;
    public float maxRotationSpeed = 180.0f;
    private string movementPattern;

    private GameObject knight = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision produced");
        if (collision.gameObject.layer == LayerMask.GetMask("Obstacle"))
        {
            Debug.Log(collision.gameObject.layer);
            direction *= -1;
        }
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
            if (direction == new Vector3(0, 0, 0))
            {
                if (movementPattern == "Vertically")
                    direction = Vector3.forward;
                else if (movementPattern == "Horizontally")
                    direction = Vector3.right;
            }
            // Translate
            transform.Translate(speed * Time.deltaTime * Vector3.Normalize(direction), Space.World);
        }
    }
}
