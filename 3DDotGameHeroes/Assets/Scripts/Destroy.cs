using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifetime = 10.0f;
    
    private void DestroyWithParticles()
    {
        gameObject.GetComponent<TriggerParticles>().TriggerParticleSystem();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Anubis"))
        {
            DestroyWithParticles();
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
