using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisMove : MonoBehaviour
{

    Vector3 prevDirection = Vector3.forward;
    Vector3 direction = Vector3.forward;
    public float maxRotationSpeed = 180.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Locate the direction to reach  the player
        Vector3 direction = GameObject.Find("Knight").transform.position - transform.position;
        direction = Vector3.Normalize(direction);

        // Rotate the enemy to face the player
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > maxRotationSpeed * Time.deltaTime) angle = maxRotationSpeed * Time.deltaTime;
        if (axis.y < 0.0f) angle = -angle;
        transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);
    }
}
