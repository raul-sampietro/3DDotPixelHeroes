using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public int CollectItem(string itemTag, int count = 1)
    {
        if (items.ContainsKey(itemTag)) items[itemTag] += count;
        else items[itemTag] = count;
        Debug.Log("CollectItem " + itemTag + ": " + items[itemTag]);

        if (itemTag == "Boomerang")
        {
            GameObject stbom = GameObject.FindGameObjectWithTag("HUD");
            stbom = stbom.transform.Find("StBoomerang").gameObject;
            TextMeshProUGUI textUI = stbom.transform.GetComponent<TextMeshProUGUI>();
            textUI.text = "Equiped";
        }
        else if (itemTag == "Key")
        {
            GameObject stkey = GameObject.FindGameObjectWithTag("HUD");
            stkey = stkey.transform.Find("StKeys").gameObject;
            TextMeshProUGUI textUI = stkey.transform.GetComponent<TextMeshProUGUI>();
            textUI.text = GetItemCount("Key").ToString();
        }
        else if (itemTag == "KeyBoss")
        {
            GameObject stkeyB = GameObject.FindGameObjectWithTag("HUD");
            stkeyB = stkeyB.transform.Find("StKeysB").gameObject;
            TextMeshProUGUI textUI = stkeyB.transform.GetComponent<TextMeshProUGUI>();
            string aux = GetItemCount("KeyBoss").ToString() + " (Boss)";
            textUI.text = aux;
        }

        else if (itemTag == "Coin")
        {
            GameObject stkeyB = GameObject.FindGameObjectWithTag("HUD");
            stkeyB = stkeyB.transform.Find("NCoins").gameObject;
            TextMeshProUGUI textUI = stkeyB.transform.GetComponent<TextMeshProUGUI>();
            textUI.text = GetItemCount("Coin").ToString();
        }

        return items[itemTag];
    }

    public int DeleteItem(string itemTag, int count = 1)
    {
        if (!items.ContainsKey(itemTag)) return 0;
        
        items[itemTag] -= count;
        if (items[itemTag] < 0) items[itemTag] = 0;
        Debug.Log("DeleteItem " + itemTag + ": " + items[itemTag]);
        return items[itemTag];        
    }

    public bool HasItem(string itemTag)
    {
        return items.ContainsKey(itemTag) && items[itemTag] > 0;
    }

    public bool HasItem(string itemTag, int count)
    {
        return items.ContainsKey(itemTag) && items[itemTag] >= count;
    }

    public int GetItemCount(string itemTag)
    {
        return items.ContainsKey(itemTag) ? items[itemTag] : 0;
    }
}
