using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingCollision : MonoBehaviour
{
    private DamageMatrix damageMatrix;

    // Start is called before the first frame update
    void Start()
    {
        damageMatrix = DamageMatrix.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int damage = damageMatrix.DoesDamage(gameObject.tag, collision.gameObject.tag);
        if (damage > 0)
        {
            collision.gameObject.GetComponent<HealthSystem>().Damage(damage);
        }
    }
}
