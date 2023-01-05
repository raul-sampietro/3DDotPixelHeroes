using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public int value = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Life")
            {
                collision.gameObject.GetComponent<HealthSystem>().Cure(value);
                Destroy(gameObject);
            }
            else collision.gameObject.GetComponent<InventoryManager>().CollectItem(gameObject, value);
        }
    }
}
