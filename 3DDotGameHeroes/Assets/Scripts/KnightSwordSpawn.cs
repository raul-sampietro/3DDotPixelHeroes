using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSwordSpawn : MonoBehaviour
{
    public float maxScale = 1.5f;
    public float minScale = 0.5f;
    public int FramesToAppear = 30;
    public int FramesToDisappear = 30;

    Transform bladeTransform;
    private float scale;

    bool appearing, appearingParent, appearingBlade;
    bool disappearing, disappearingParent, disappearingBlade;

    float appearRate;
    float disappearRate;

    // Start is called before the first frame update
    void Start()
    {
        appearing = appearingParent = appearingBlade = true;
        disappearing = disappearingParent = disappearingBlade = false;

        float normHP = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>().GetNormalizedHP();
        float diffScale = maxScale - minScale;
        scale = minScale + (diffScale * normHP);

        appearRate = scale / (float)FramesToAppear;
        disappearRate = scale / (float)FramesToDisappear;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);

        bladeTransform = transform.GetChild(0).GetChild(0).GetChild(0);
        bladeTransform.localScale = new Vector3(bladeTransform.localScale.x, bladeTransform.localScale.y, 0);
    }

    public void Disappear()
    {
        disappearing = disappearingParent = disappearingBlade = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (appearing)
        {
            // Scale parent
            if (transform.localScale.z + appearRate > 1.0f)
            {
                transform.localScale += new Vector3(0, 0, appearRate - (transform.localScale.z + appearRate - 1.0f));
                appearingParent = false;
            }
            else
            {
                transform.localScale += new Vector3(0, 0, appearRate);
            }

            // Scale blade
            if (bladeTransform.localScale.z + appearRate > scale)
            {
                bladeTransform.localScale += new Vector3(0, 0, appearRate - (transform.localScale.z + appearRate - scale));
                appearingBlade = false;
            }
            else {
                bladeTransform.localScale += new Vector3(0, 0, appearRate);
            }

            if (!appearingBlade && !appearingParent) appearing = false;
        }
        else if (disappearing)
        {
            // Scale parent
            if (transform.localScale.z - disappearRate < 0)
            {
                disappearingParent = false;
            }
            else
            {
                transform.localScale -= new Vector3(0, 0, disappearRate);
            }

            // Scale blade
            if (bladeTransform.localScale.z - disappearRate > 0)
            {
                bladeTransform.localScale -= new Vector3(0, 0, disappearRate);
            }
            else disappearingBlade = false;

            if (!disappearingBlade && !disappearingParent) Destroy(this.gameObject);
        }
    }
}
