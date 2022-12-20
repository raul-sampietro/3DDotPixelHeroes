using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawn : MonoBehaviour
{
    public int framesToAppear = 30;
    public float verticalDiff = 10;
    public float initialRotationSpeedDeg = 10;
    public float finalRotationSpeedDeg = 2;

    Animator animator;
    float finalHeight;
    float currentRotationSpeed;
    float scaleRate, translateRate, rotateRate;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        transform.localScale = new Vector3(0, 0, 0);
        finalHeight = transform.position.y + verticalDiff;
        scaleRate = 1f / (float)framesToAppear;
        translateRate = (float)verticalDiff / (float)framesToAppear;
        rotateRate = (finalRotationSpeedDeg - initialRotationSpeedDeg) / (float)framesToAppear;
        currentRotationSpeed = initialRotationSpeedDeg;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale
        if (transform.localScale.x < 1f)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleRate, transform.localScale.y + scaleRate, transform.localScale.z + scaleRate);
        }
        //Translate
        if (transform.position.y < finalHeight)
        {
            transform.Translate(0, translateRate, 0);
        }
        //Rotate
        if (currentRotationSpeed > finalRotationSpeedDeg)
        {
            currentRotationSpeed += rotateRate;
        }
        transform.Rotate(transform.up, currentRotationSpeed);
    }
}
