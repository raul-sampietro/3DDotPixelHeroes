using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maximumHP = 50;

    int currentHP;
    bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = 20;
    }

    public void SwitchInvincibility()
    {
        isInvincible = !isInvincible;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Whether it has been killed or not</returns>
    public bool Kill()
    {
        if (!isInvincible)
            currentHP = 0;
        return currentHP == 0;
    }

    public int Damage(int damage)
    {
        if (!isInvincible)
            currentHP = currentHP - damage < 0 ? 0 : currentHP - damage;
        return currentHP;
    }

    public int Cure(int hp)
    {
        currentHP = currentHP + hp > maximumHP ? maximumHP : currentHP + hp;
        return currentHP;
    }

    public int GetHP()
    {
        return currentHP;
    }

    public float GetNormalizedHP()
    {
        return (float)currentHP / (float)maximumHP;
    }

    public int GetMaxHP()
    {
        return maximumHP;
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            //Kill
        }
    }
}