using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifetime = 10.0f;
    
    private void DestroyWithParticles()
    {
        gameObject.BroadcastMessage("TriggerParticleSystem");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 3 is the obstable layer number
        if (collision.gameObject.layer == 3)
        {
            DestroyWithParticles();
        }
        else if (collision.gameObject.layer == 7)
        {
            DestroyWithParticles();
            // Aditional features like health
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
        {
            DestroyWithParticles();
        }
    }
}
