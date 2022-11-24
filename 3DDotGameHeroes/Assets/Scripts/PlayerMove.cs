using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float Speed = 15;
    Vector3 prevDirection = Vector3.forward;
    Vector3 direction = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Compose direction
        Vector3 direction = new(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (direction != new Vector3(0, 0, 0))
        {
            gameObject.GetComponent<Animator>().Play("Walk");
        }
        else gameObject.GetComponent<Animator>().Play("Idle");
        // Rotate to face direction
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (axis.y < 0.0f) angle = -angle;
        transform.Rotate(new Vector3(0,1,0), angle, Space.World);
        // Translate
        transform.Translate(Speed * Time.deltaTime * Vector3.Normalize(direction), Space.World);
        prevDirection = direction;
    }
}
