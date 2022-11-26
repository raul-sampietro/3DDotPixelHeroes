using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisShoots : MonoBehaviour
{
    public float shootingFreq = 1f;
    float timeToShoot;
    public GameObject shot;
    public float shotSpeed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        timeToShoot = 1.0f / shootingFreq;
    }

    // Update is called once per frame
    void Update()
    {
        timeToShoot -= Time.deltaTime;
        if (timeToShoot < 0.0f)
        {
            // Create the shot
            timeToShoot = 1.0f / shootingFreq;
            GameObject obj = Instantiate(shot, transform.position - new Vector3(1.5f, 0.0f, 0.0f), transform.rotation);

            // Set the direction of the shot
            Vector3 direction = GameObject.Find("knight").transform.position - transform.position;
            direction = Vector3.Normalize(direction) * shotSpeed;
            obj.GetComponent<Rigidbody>().velocity = direction;
            
            // Start the animation of the shot
            obj.GetComponent<Animator>().Play("Idle");
        }
    }
}
