using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMatrix : Singleton<DamageMatrix>
{
    private Dictionary<string, Dictionary<string, bool>> damageMatrix;
    private Dictionary<string, int> damageValues;

    private void Awake()
    {
        damageMatrix = new Dictionary<string, Dictionary<string, bool>>
        {
            // Weapons
            ["KnightSword"] = new Dictionary<string, bool>
            {
                ["Anubis"] = true,
            },

            // Enemies
            ["Anubis"] = new Dictionary<string, bool>
            {
                ["Player"] = true,
            },

            // Enemy weapons
            ["AnubisShot"] = new Dictionary<string, bool>
            {
                ["Player"] = true,
            },
        };

        damageValues = new Dictionary<string, int>
        {
            // Weapons
            ["KnightSword"] = 15,

            // Enemies
            ["Anubis"] = 5,

            // Enemy Weapons
            ["AnubisShot"] = 10,
        };
    }

    public int DoesDamage(string fromTag, string toTag)
    {
        if (!damageMatrix.ContainsKey(fromTag)) return 0;
        if (!damageMatrix[fromTag].ContainsKey(toTag)) return 0;
        if (!damageMatrix[fromTag][toTag]) return 0;
        return damageValues[fromTag];
    }
}

