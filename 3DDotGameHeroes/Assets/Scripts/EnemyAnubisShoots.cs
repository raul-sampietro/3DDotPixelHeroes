using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisShoots : MonoBehaviour
{
    public float shootingFreq = 0.25f;
    float timeToShoot;
    public GameObject shot;

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
            Instantiate(shot, transform.position + new Vector3(0.0f, 10.0f, 0.0f), transform.rotation);
        }
    }
}
