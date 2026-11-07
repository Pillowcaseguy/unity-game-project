using UnityEngine;

public class SoldierStickyFireBehavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public GameObject ground_sticky_fire;
    public bool created_ground_fire;
    public Rigidbody2D rb;
    public bool started_falling;
    public bool touched_player;
    public EnemyAttack attack;
    public Animator ani;
    public LevelManager_TestBoss5 manager;
    public LayerMask ground_layer;
    public int enemy_layer;
    public float lifetime;
    public void Awake()
    {
        ani = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-150f, 150f), 300f));
        ground_layer = LayerMask.GetMask("Ground");
        enemy_layer = LayerMask.NameToLayer("Enemy");
        lifetime = 5;
    }
    public void Update()
    {
        if ((attack.wasHit || (attack.wasBlocked && manager.player_life.trueInvincibilityTime <= 0)) && !touched_player)
        {
            touched_player = true;
            manager.apply_super_burn(12f);
        }

        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = 1.5f;
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void hit_ground()
    {
        if (!created_ground_fire)
        {
            if (ani.GetBool("fire_put_out"))
            {
                created_ground_fire = true;
                Destroy(gameObject);
            }
            else
            {
                Instantiate(ground_sticky_fire, new Vector2(transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)).GetComponent<SoldierStickyFireGroundBehavior>().manager = manager;
                created_ground_fire = true;
                Destroy(gameObject);
            }
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (player_attack.attackType == PlayerAttack.AttackType.Ice && player_attack.damage >= 40)
        {
            ani.SetBool("fire_put_out", true);
            gameObject.layer = enemy_layer;
        }
    }

    public void TakeDamage(int damage)
    {
        
    }

}
