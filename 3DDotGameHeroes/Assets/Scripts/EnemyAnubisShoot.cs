using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisShoot : MonoBehaviour
{
    public float shootingFreq = 0.25f;
    public float timeToShoot;
    public GameObject shot;

    private GameObject knight = null;

    // Start is called before the first frame update
    void Start()
    {
        timeToShoot = 1.0f / shootingFreq;
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
            knight = GameObject.Find("Knight");

        timeToShoot -= Time.deltaTime;

        if (!Physics.Linecast(transform.position, knight.transform.position))
        {
            if (timeToShoot < 0.0f)
            {
                // Create the shot
                timeToShoot = 1.0f / shootingFreq;
                Instantiate(shot, transform.position + transform.forward + new Vector3(0.0f, 20.0f, 0.0f), transform.rotation);

            }
            if (timeToShoot < 0.5f)
            {
                // Start the shooting animation
                gameObject.GetComponent<Animator>().Play("shoot");
            }
        }
    }
}
