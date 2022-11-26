using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFollowsPlayer : MonoBehaviour
{
    public float shotSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Start the animation of the shot
        GetComponent<Animator>().Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        // Set the direction of the shot
        Vector3 direction = GameObject.Find("knight").transform.position - transform.position;
        direction = Vector3.Normalize(direction) * shotSpeed;
        GetComponent<Rigidbody>().velocity = direction;
    }
}
