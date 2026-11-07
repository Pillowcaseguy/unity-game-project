using UnityEngine;

public class TestBoss5Ninja : MonoBehaviour
{
    public float hit_delay;
    public float lifetime;
    public EnemyAttack my_attack;
    private void Awake()
    {
        my_attack = GetComponent<EnemyAttack>();
        lifetime = 25f;
        if (transform.position.x > 13)
        {
            transform.position = new Vector2(13, transform.position.y);
        }
        else if (transform.position.x < -13)
        {
            transform.position = new Vector2(-13, transform.position.y);
        }
    }
    void Update()
    {
        if (my_attack.wasHit || my_attack.wasBlocked)
        {
            my_attack.clearFlags();
            hit_delay = 3f;
            my_attack.DisableHitbox();
        }

        hit_delay -= Time.deltaTime;
        if (hit_delay < 0)
        {
            my_attack.EnableHitbox();
            hit_delay = 0;
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
