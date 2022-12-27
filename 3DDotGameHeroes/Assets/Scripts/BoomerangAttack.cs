using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BoomerangState
{
    NONE,
    SPAWNING,
    ATTACKING,
    RETURNING,
    DESPAWNING
}

public class BoomerangAttack : MonoBehaviour
{
    public float speed = 30;
    public float range = 96;
    public float framesToSpawn = 4;

    Vector3 startPos;
    Vector3 startDirection;
    float maxScale = 1f;
    float scaleRate;
    float despawnDistance;

    GameObject player;
    BoomerangState state;
    DamageMatrix damageMatrix;

    // Start is called before the first frame update
    void Start()
    {
        state = BoomerangState.NONE;
        damageMatrix = DamageMatrix.Instance;
    }

    public void Initialize(GameObject player, Vector3 startPos, Vector3 startDirection)
    {
        this.player = player;
        this.startPos = startPos;
        this.startDirection = Vector3.Normalize(startDirection);
        state = BoomerangState.SPAWNING;
        transform.localScale = Vector3.zero;
        transform.position = startPos;
        scaleRate = maxScale / (float)framesToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BoomerangState.NONE) return;
        else if (state == BoomerangState.SPAWNING || state == BoomerangState.ATTACKING)
        {
            if (state == BoomerangState.SPAWNING)
            {
                if (transform.localScale.x >= maxScale)
                {
                    despawnDistance = Vector3.Distance(startPos, transform.position);
                    state = BoomerangState.ATTACKING;
                }    
                // Increase scale
                transform.localScale = new Vector3(transform.localScale.x + scaleRate,
                                                   transform.localScale.y + scaleRate,
                                                   transform.localScale.z + scaleRate);
            }
            else if (state == BoomerangState.ATTACKING)
            {
                if (Vector3.Distance(startPos, transform.position) >= range)
                    state = BoomerangState.RETURNING;
            }

            // Common: Move on to the startDirection
            transform.Translate(Time.deltaTime * speed * startDirection);
        }
        else if (state == BoomerangState.RETURNING || state == BoomerangState.DESPAWNING)
        {
            if (state == BoomerangState.RETURNING)
            {
                // if (distance <= minDist) state = BoomerangState.DESPAWNING
                if (Vector3.Distance(transform.position, player.transform.position) <= despawnDistance)
                    state = BoomerangState.DESPAWNING;
            }
            else if (state == BoomerangState.DESPAWNING)
            {
                // Reduce scale
                if (transform.localScale.x <= 0)
                    Destroy(gameObject);
                transform.localScale = new Vector3(transform.localScale.x - scaleRate,
                                                   transform.localScale.y - scaleRate,
                                                   transform.localScale.z - scaleRate);
            }

            // Common: Move back to the player
            Vector3 returnDirection = player.transform.position - transform.position;
            returnDirection.y = 0f;
            transform.Translate(Time.deltaTime * speed * returnDirection);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);
        int damage = damageMatrix.DoesDamage(gameObject.tag, collision.gameObject.tag);
        if (damage > 0)
            collision.gameObject.GetComponent<HealthSystem>().Damage(damage);
    }
}
