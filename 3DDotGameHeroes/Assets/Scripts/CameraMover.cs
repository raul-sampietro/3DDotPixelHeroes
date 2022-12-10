using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
            cam = GameObject.Find("OverviewCamera").GetComponent<Camera>();

        cam.transform.Translate(new Vector3(1, 0, 0));
    }
}
