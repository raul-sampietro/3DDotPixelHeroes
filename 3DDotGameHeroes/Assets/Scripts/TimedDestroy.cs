using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float lifetime = 10.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            Destroy(gameObject);
    }
}
