using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float shotSpeed = 30.0f;

    private GameObject knight = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
            knight = GameObject.Find("Knight");

        // Set the direction
        Vector3 direction = knight.transform.position - transform.position;
        direction = Vector3.Normalize(direction) * shotSpeed;
        direction = new Vector3(direction.x, 0, direction.z);
        GetComponent<Rigidbody>().velocity = direction;
    }
}
