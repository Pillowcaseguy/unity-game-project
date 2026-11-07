using UnityEngine;

public class NinjaTornadoBehavior : MonoBehaviour, EnemyDamageable
{
    public EnemyAttack my_attack;
    public bool attack_hit;
    public LevelManager_TestBoss5 manager;
    void Awake()
    {
        my_attack = GetComponent<EnemyAttack>();
    }
    public void re_enable_attack()
    {
        attack_hit = false;
        my_attack.clearFlags();
        my_attack.disableAttackCycle = false;
        my_attack.EnableHitbox();
    }
    public void add_weakness()
    {
        my_attack.weaknessPotency = 0.6f;
        my_attack.weaknessDuration = 5f;
        my_attack.debuffEvenIfBlocked = true;
    }
    public void add_damage()
    {
        my_attack.damage = 2;
    }
    public void explode_particles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var particle = Instantiate(manager.hit_particle, manager.player.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SpriteRenderer>();
            particle.color = Color.white;
        }
    }
    public void destroy_me()
    {
        var hi = Camera.main.GetComponent<CameraShake>();
        if (hi != null)
        {
            hi.shakeOnHurt(1f);
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        if (my_attack.wasHit && !attack_hit)
        {
            explode_particles(20);
            attack_hit = true;
        }
    }

    public void TakeDamage(int damage)
    {
        
    }

    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isBlock();
    }
}
