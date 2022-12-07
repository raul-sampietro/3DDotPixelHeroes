using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float shotSpeed = 30.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Set the direction
        Vector3 direction = GameObject.Find("Knight").transform.position - transform.position;
        direction = Vector3.Normalize(direction) * shotSpeed;
        direction = new Vector3(direction.x, 0, direction.z);
        GetComponent<Rigidbody>().velocity = direction;
    }
}
