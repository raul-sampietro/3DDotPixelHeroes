using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    // Identifier
    public Color color;
    
    // Asset mapped
    public GameObject prefab;

    // Drop options
    public bool dropCoin;
    public bool dropLife;

    // Type of movement
    public System.String movementPattern;
}
