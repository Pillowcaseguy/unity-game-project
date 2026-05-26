using UnityEngine;

public class FirePoolBehavior : MonoBehaviour
{
    public float lifetime;
    public float hit_delay;
    public EnemyAttack attack;
    void Start()
    {
        lifetime = 15f;
        attack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((attack.wasBlocked || attack.wasHit) && hit_delay <= 0f)
        {
            attack.clearFlags();
            attack.disableAttackCycle = false;
            attack.EnableHitbox();
            hit_delay = 0.5f;
        }

        hit_delay -= Time.deltaTime;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
