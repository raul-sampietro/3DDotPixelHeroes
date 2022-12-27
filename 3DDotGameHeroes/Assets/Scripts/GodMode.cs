using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    public string normalKeyTag;
    public string bossKeyTag;
    public string boomerangTag;

    InventoryManager inventory;
    HealthSystem health;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<InventoryManager>();
        health = GetComponent<HealthSystem>();   
    }

    // Update is called once per frame
    void Update()
    {
        // Invincibility
        if (Input.GetKeyDown(KeyCode.G))
        {
            health.SwitchInvincibility();
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
        // Get Boomerang
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.CollectItem(boomerangTag);
        }
    }
}
