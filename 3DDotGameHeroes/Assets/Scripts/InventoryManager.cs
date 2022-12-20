using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    Dictionary<string, int> items;

    void Start()
    {
        items = new Dictionary<string, int>();    
    }

    /// <summary>
    /// The item is added to the inventory and Destroy is called on it.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="count"></param>
    /// <returns>The number of items it has with the same tag as gameObject.</returns>
    public int CollectItem(GameObject gameObject, int count = 1) {
        int res;
        if (gameObject.GetComponent<CollectibleObject>().CanBeCollected())
            res = CollectItem(gameObject.tag, count);
        else res = 0;
        Destroy(gameObject);
        // if (gameObject.tag in [Key, KeyBoss, Boomerang])
            // Instantiate above head
            // Move camera
        return res;
    }

    private int CollectItem(string itemTag, int count = 1)
    {
        if (items.ContainsKey(itemTag)) items[itemTag] += count;
        else items[itemTag] = count;
        Debug.Log("CollectItem " + itemTag + ": " + items[itemTag]);
        return items[itemTag];
    }

    public int DeleteItem(string itemTag, int count = 1)
    {
        if (!items.ContainsKey(itemTag)) return 0;
        
        items[itemTag] -= count;
        if (items[itemTag] < 0) items[itemTag] = 0;
        return items[itemTag];        
    }

    public int HasItem(string itemTag)
    {
        if (!items.ContainsKey(itemTag)) return 0;
        return items[itemTag];
    }
}
