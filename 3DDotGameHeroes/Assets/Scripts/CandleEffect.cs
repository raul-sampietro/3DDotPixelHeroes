using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleEffect : MonoBehaviour
{
    private Light torchLight;

    public float minIntensity;
    public float maxIntesity;

    public float minRange;
    public float maxRange;
    // Start is called before the first frame update
    void Start()
    {
        torchLight = gameObject.transform.GetChild(1).gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Random.Range(0,4)) == 0)
        {
            //Debug.Log((int)(Time.deltaTime * 10000));
            torchLight.intensity = Random.Range(minIntensity, maxIntesity);
            torchLight.range = Random.Range(minRange, maxRange);
        }

    }
}
