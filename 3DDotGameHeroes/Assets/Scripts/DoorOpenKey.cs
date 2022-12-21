using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenKey : MonoBehaviour
{
    public GameObject keyObject;

    Animator animator;
    bool opening;
    string keyTag;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        opening = false;
        if (keyObject != null) keyTag = keyObject.tag;
        else Debug.LogWarning("No Key has been assigned to this DoorOpenKey");
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collided = collision.gameObject;
        if (!opening && collided.layer == 7 && (keyObject == null
            || collided.GetComponent<InventoryManager>().HasItem(keyTag)))
        {
            animator.SetBool("isOpening", true);
            if (keyObject != null) collided.GetComponent<InventoryManager>().DeleteItem(keyTag);
            GetComponent<BoxCollider>().enabled = false;
            opening = true;
        }
    }
}
