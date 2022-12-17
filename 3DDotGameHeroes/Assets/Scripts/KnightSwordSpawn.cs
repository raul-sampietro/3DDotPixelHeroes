using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSwordSpawn : MonoBehaviour
{
    public float Scale = 1;
    public int FramesToAppear = 30;
    public int FramesToDisappear = 30;

    bool appearing;
    bool disappearing;

    float appearRate; 
    float disappearRate;

    // Start is called before the first frame update
    void Start()
    {
        appearing = true;
        disappearing = false;
        disappearing = false;

        appearRate = Scale / (float)FramesToAppear;
        disappearRate = Scale / (float)FramesToDisappear;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);
    }

    public void Disappear()
    {
        disappearing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (appearing)
        {
            if (transform.localScale.z + appearRate > Scale)
            {

                transform.localScale += new Vector3(0, 0, appearRate - (transform.localScale.z + appearRate - Scale));
                appearing = false;
            }
            else {
                transform.localScale += new Vector3(0, 0, appearRate);
            }

        }
        else if (disappearing)
        {
            if (transform.localScale.z - disappearRate < 0)
            {
                transform.localScale -= new Vector3(0, 0, disappearRate - (disappearRate - transform.localScale.z));
                Destroy(this.gameObject);
            }
            else {
                transform.localScale -= new Vector3(0, 0, disappearRate);
            }
        }
    }
}
