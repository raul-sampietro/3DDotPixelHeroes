using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnubisShoot : MonoBehaviour
{
    public float shootingFreq = 0.25f;
    public float timeToShoot;
    public GameObject shot;

    private GameObject knight = null;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        timeToShoot = 1.0f / shootingFreq;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
            knight = GameObject.Find("Knight");

        // TODO Eliminar el delta time, cambiar por un random como en el candle effect
        timeToShoot -= Time.deltaTime;

        // A valorar: hacerlo con pase de mensajes desde move hacia shoot
        if (Physics.Linecast(transform.position, knight.transform.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject == knight)
            {
                if (timeToShoot < 0.0f)
                {
                    // Trigger the shooting animation
                    animator.SetBool("isShooting", true);

                    // Create the shot
                    timeToShoot = 1.0f / shootingFreq;
                    Instantiate(shot, transform.position + transform.forward + new Vector3(0.0f, 5.0f, 0.0f), transform.rotation);
                }
            }
        }
    }
}
