using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enter");    
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");
    }

    void Update()
    {
        
    }
}
