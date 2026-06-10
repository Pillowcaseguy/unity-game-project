using UnityEngine;

public class SwordsmanAI : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{

    public bool isStanding = false;
    public bool isAttacking = false;

    public float attackTimer;
    public float timeSinceLastAttack;
    public float deathTimer;

    private BoxCollider2D col;
    private Animator ani;
    private SpriteRenderer sprite;
    private SwordSlashAttack swash;

    public int max_health;
    public int health;

    public float damage_color_cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isStanding = true;
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        swash = GetComponentInChildren<SwordSlashAttack>();
        max_health = 100;
        health = max_health;
        timeSinceLastAttack = 0f;
        damage_color_cooldown = 0.15f;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (health > 0)
        {
            if (!isAttacking)
            {

                if (timeSinceLastAttack > 5.0f)
                {
                    startAttacking();
                    //attackTimer = 0.5f;
                }

            }

            /*if (attackTimer > 0f)
            {

                attackTimer -= Time.deltaTime;

                if (attackTimer < 0f)
                {
                    attackTimer = 0f;
                }

            }*/

            /*if (attackTimer == 0f && isAttacking)
            {
                stopAttacking();
            }*/

        }
        else
        {
            Destroy(gameObject);
        }

        if (sprite.color != Color.white)
        {
            damage_color_cooldown -= Time.deltaTime;
            if (damage_color_cooldown <= 0f)
            {
                damage_color_cooldown = 0.15f;
                sprite.color = Color.white;
            }
        }

        timeSinceLastAttack += Time.deltaTime;

        ani.SetBool("isStanding", isStanding);
        ani.SetBool("isAttacking", isAttacking);
        //ani.SetBool("isDead", health > 0);

    }
    public void stopAttacking()
    {
        isAttacking = false;
        isStanding = true;
        timeSinceLastAttack = 0f;
        end_slash();
    }
    public void startAttacking()
    {
        isAttacking = true;
        isStanding = false;
    }
    public void start_slash()
    {
        swash.start_attack();
    }
    public void end_slash()
    {
        swash.end_attack();
    }
    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isHit();
        TakeDamage(player_attack.damage);
    }
}
