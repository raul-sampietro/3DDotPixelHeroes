using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawn : MonoBehaviour
{
    public int framesToAppear = 30;
    public float verticalDiff = 10;
    public float initialRotationSpeedDeg = 10;
    public float finalRotationSpeedDeg = 2;

    CollectibleObject collectibleObj;
    float finalHeight;
    float currentRotationSpeed;
    float scaleRate, translateRate, rotateRate;
    bool scaleDone, translateDone, rotateDone;

    // Start is called before the first frame update
    void Start()
    {
        collectibleObj = GetComponent<CollectibleObject>();
        collectibleObj.SetCollectibility(false);
        transform.localScale = new Vector3(0, 0, 0);
        finalHeight = transform.position.y + verticalDiff;
        scaleRate = 1f / (float)framesToAppear;
        translateRate = (float)verticalDiff / (float)framesToAppear;
        rotateRate = (finalRotationSpeedDeg - initialRotationSpeedDeg) / (float)framesToAppear;
        currentRotationSpeed = initialRotationSpeedDeg;
        scaleDone = translateDone = rotateDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale
        if (transform.localScale.x < 1f)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleRate, transform.localScale.y + scaleRate, transform.localScale.z + scaleRate);
        }
        else if (!scaleDone) scaleDone = true;
        //Translate
        if (transform.position.y < finalHeight)
        {
            transform.Translate(0, translateRate, 0);
        }
        else if (!translateDone) translateDone = true;
        //Rotate
        if (currentRotationSpeed > finalRotationSpeedDeg)
        {
            currentRotationSpeed += rotateRate;
        }
        else if (!rotateDone) rotateDone = true;
        transform.Rotate(transform.up, currentRotationSpeed);

        // When animation has finished, the key can be collected
        if (scaleDone && translateDone && rotateDone && !collectibleObj.CanBeCollected())
            collectibleObj.SetCollectibility(true);
    }
}
