using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public GameObject item;
    public float itemSpawnY = 4;

    Animator animator;
    bool opened, itemTaken;
    GameObject itemObj;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        opened = itemTaken = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 playerDirection = Vector3.Normalize(collision.gameObject.transform.position - transform.position);
        float angle = Vector3.Angle(playerDirection, transform.forward);

        if (collision.gameObject.layer == 7 && angle < 40)
        {
            if (!opened)
            {
                animator.SetBool("isOpening", true);
                opened = true;
                itemObj = Instantiate(item, transform.position + (transform.up * itemSpawnY), transform.rotation);
            }
            else if (!itemTaken && itemObj.GetComponent<CollectibleObject>().CanBeCollected()) {
                Debug.Log("Chest collect key");
                collision.gameObject.GetComponent<InventoryManager>().CollectItem(itemObj);
                itemTaken = true;
            }
        }
    }
}
