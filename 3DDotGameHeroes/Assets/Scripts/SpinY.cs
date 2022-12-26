using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinY : MonoBehaviour
{
    public float rotationSpeed = 30.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed/100, Space.World);
    }
}
