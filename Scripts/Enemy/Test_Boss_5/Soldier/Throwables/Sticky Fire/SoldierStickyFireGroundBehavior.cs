using UnityEngine;

public class SoldierStickyFireGroundBehavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public EnemyAttack attack;
    public Animator ani;
    public LevelManager_TestBoss5 manager;
    public BoxCollider2D box;
    public float lifetime;
    public LayerMask enemy_layer;
    public void Awake()
    {
        ani = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
        box = GetComponent<BoxCollider2D>();
        lifetime = 20;
        enemy_layer = LayerMask.NameToLayer("Enemy");
    }
    public void Update()
    {
        if (manager.ground_fire_hit_cooldown <= 0 && !ani.GetBool("fire_put_out"))
        {
            attack.EnableHitbox();
        }
        else
        {
            gameObject.layer = enemy_layer;
            box.enabled = false;
        }

        if (attack.wasHit)
        {
            manager.apply_super_burn(8f);
            manager.ground_fire_hit_cooldown = 3f;
            attack.clearFlags();
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (player_attack.attackType == PlayerAttack.AttackType.Ice && player_attack.damage >= 40)
        {
            ani.SetBool("fire_put_out", true);
        }
    }

    public void TakeDamage(int damage)
    {

    }
}
