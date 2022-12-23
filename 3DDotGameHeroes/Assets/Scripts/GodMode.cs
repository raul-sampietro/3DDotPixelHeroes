using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    public string normalKeyTag;
    public string bossKeyTag;

    bool isInvincible = false;
    InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<InventoryManager>();   
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    // Update is called once per frame
    void Update()
    {
        // Invincibility
        if (Input.GetKeyDown(KeyCode.G))
        {
            isInvincible = !isInvincible;
        }
        // Get Normal Key
        if (Input.GetKeyDown(KeyCode.K))
        {
            inventory.CollectItem(normalKeyTag);
        }
        // Get Boss Key
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.CollectItem(bossKeyTag);
        }
    }
}
