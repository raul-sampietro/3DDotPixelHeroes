using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisShoot : MonoBehaviour
{
    public float shootingFreq = 0.25f;
    public float timeToShoot;
    public GameObject shot;

    private GameObject knight = null;
    private LayerMask playerLayer; 

    // Start is called before the first frame update
    void Start()
    {
        timeToShoot = 1.0f / shootingFreq;
        playerLayer = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
            knight = GameObject.Find("Knight");

        timeToShoot -= Time.deltaTime;

        // A valorar: hacerlo con pase de mensajes desde move hacia shoot
        Physics.Linecast(transform.position, knight.transform.position, out RaycastHit hit);
        if (Physics.Linecast(transform.position, knight.transform.position, out hit))
        {
            if (hit.collider.gameObject == knight)
            {
                if (timeToShoot < 0.0f)
                {
                    // Create the shot
                    timeToShoot = 1.0f / shootingFreq;
                    Instantiate(shot, transform.position + transform.forward + new Vector3(0.0f, 5.0f, 0.0f), transform.rotation);
                }
                if (timeToShoot < 0.5f)
                {
                    // Start the shooting animation
                    //gameObject.GetComponent<Animation>().Play("anubis_shoot");
                }
            }
        }
        else
        {
            // Trigger the walking animation
           //gameObject.GetComponent<Animation>().Play("anubis_walking");
        }
    }
}
