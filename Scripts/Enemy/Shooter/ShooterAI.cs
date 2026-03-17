using UnityEngine;

public class ShooterAI : MonoBehaviour, EnemyDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isStanding = false;
    public bool isAttacking = false;
    public bool isAttackingOne = false;
    public bool isAttackingThree = false;
    public bool isAttackingMelee = false;
    public bool playerInRange = false;
    public bool isDead = false;
    public int health;
    public int starting_health = 200;

    public float timeSinceLastAttack;
    public float deathTimer;
    public int lastAttackUsed = 0;
    public float stunTime = 0f;
    public float attackTime = 0f;

    public float bullet_speed = 5;

    public GameObject player;

    public ShooterMelee melee_control;

    private BoxCollider2D col;
    private Animator ani;
    public SpriteRenderer sprite;

    public ShooterShootPoint shootPoint;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        shootPoint = GetComponentInChildren<ShooterShootPoint>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isStanding = true;
        health = starting_health;
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        melee_control = GetComponentInChildren<ShooterMelee>();
        timeSinceLastAttack = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            deathTimer = 2.0f;
            Die();
        }

        if (!isDead && sprite.color.a > 0.5)
        {

            if (!isAttacking && stunTime <= 0f)
            {

                if (playerInRange)
                {
                    if (timeSinceLastAttack >= 6.0f)
                    {
                        timeSinceLastAttack = 4.25f;
                    }

                    isAttacking = true;
                    isAttackingMelee = true;
                    isStanding = false;

                }

                if (timeSinceLastAttack > 6.0f)
                {
                    isAttacking = true;
                    isStanding = false;
                    if (lastAttackUsed != 1) // lastAttackUsed! 1 = One Shot, 2 = Three Shot
                    {
                        isAttackingOne = true;
                        lastAttackUsed = 1;
                    }
                    else
                    {
                        isAttackingThree = true;
                        lastAttackUsed = 2;
                    }

                }

            }

            if (stunTime > 0f)
            {

                stunTime -= Time.deltaTime;

                if (stunTime < 0f)
                {
                    stunTime = 0f;
                }

            }

        }

        if (!isDead)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= 3.0f)
            {
                stopAttacking();
                attackTime = 0f;
            }
        }
        else
        {
            attackTime = 0f;
        }

        ani.SetBool("isStanding", isStanding);
        ani.SetBool("isAttacking", isAttacking);
        ani.SetBool("isAttackingMelee", isAttackingMelee);
        ani.SetBool("isAttackingOne", isAttackingOne);
        ani.SetBool("isAttackingThree", isAttackingThree);
        ani.SetBool("isStunned", stunTime > 0f);
        ani.SetBool("isDead", isDead);

    }
    void Die()
    {
        stopAttacking();
        timeSinceLastAttack = 0f;
        Destroy(gameObject, deathTimer);
    }

    void stopAttacking()
    {
        if (!isAttackingMelee)
        {
            timeSinceLastAttack = 0f;
        }
        isAttacking = false;
        isAttackingThree = false;
        isAttackingOne = false;
        isAttackingMelee = false;
        isStanding = true;
    }
    void start_melee()
    {
        melee_control.startAttack();
    }

    void end_melee()
    {
        melee_control.endAttack();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Hit(PlayerAttack player_attack)
    {
        if (sprite.color.a <= 0.5f)
            return;
        player_attack.isHit();
        int multiplier = 1;
        if (stunTime > 0f) // Double damage if stunned
            multiplier = 2;
        TakeDamage(player_attack.damage * multiplier);
    }
}
