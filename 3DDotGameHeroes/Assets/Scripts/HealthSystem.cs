using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maximumHP = 50;

    public GameObject life;
    public GameObject coin1;
    public GameObject coin2;
    public GameObject coin3;

    int currentHP;
    bool isInvincible = false;

    private GameObject nlife = null;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maximumHP;
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
        {
            currentHP = currentHP - damage < 0 ? 0 : currentHP - damage;
            Debug.Log("Damage " + tag + " -" + damage + " -> " + currentHP);
            
            if (gameObject.CompareTag("Player"))
            {
                if (nlife == null)
                {
                    nlife = GameObject.FindGameObjectWithTag("HUD");
                    nlife = nlife.transform.Find("NLife").gameObject;
                }
                int life = GetHP();
                TextMeshProUGUI textUI = nlife.transform.GetComponent<TextMeshProUGUI>();
                textUI.text = life.ToString();                
            }
        }
        return currentHP;
    }

    public int Cure(int hp)
    {
        currentHP = currentHP + hp > maximumHP ? maximumHP : currentHP + hp;
        Debug.Log("Heal " + tag + " +" + hp + " -> " + currentHP);
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
            if (gameObject.CompareTag("Player"))
            {
                //gameObject.GetComponent<TriggerParticles>().TriggerParticleSystem();
                //Destroy(gameObject);
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y = 4;
                int n = (int)Random.Range(0, 100);
                if (n < 30)
                {
                    // Life
                    Instantiate(life, pos, Quaternion.identity);
                }
                else if (n < 70)
                {
                    // Bronze Coin
                    Instantiate(coin1, pos, Quaternion.identity);

                }
                else if (n < 90)
                {
                    // Silver Coin
                    Instantiate(coin2, pos, Quaternion.identity);
                }
                else if (n < 100)
                {
                    // Gold Coin
                    Instantiate(coin3, pos, Quaternion.identity);
                }

                gameObject.GetComponent<Enemy>().DestroyWithParticles();
            }

        }
    }
}