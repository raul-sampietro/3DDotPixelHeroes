using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour
{

    public ParticleSystem particles;

    public void TriggerParticleSystem()
    {
        //Debug.Log("Se hace el trigger");
        Instantiate(particles, transform.position + new Vector3(0, 2, 0), Quaternion.Euler(90f, 0f, 0f));
        particles.Play();
    }
}
