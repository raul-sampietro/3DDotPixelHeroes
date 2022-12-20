using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used to control when an item can be collected by the player.
/// Before collecting it, we must be sure that a certain item can be collected.
/// </summary>
public class CollectibleObject : MonoBehaviour
{
    bool canBeCollected;

    public void SetCollectibility(bool value)
    {
        canBeCollected = value;
    }
    public bool CanBeCollected()
    {
        return canBeCollected;
    }
}
