using UnityEngine;

public class FoxmanBehavior : MonoBehaviour, EnemyDamageable
{
    public bool running_left;
    public bool turning_around;
    public int death_count;
    public bool just_died;
    public bool is_reviving;
    public float death_time;
    public float standing_time;
    public bool is_dying_for_real;

    private Animator ani;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private EnemyAttack FOXMAN_ATTACK;

    public float starting_x;
    public Vector2 current_position;
    public float max_move_range;

    public int max_health = 200;
    public int health = 200;

    public int active_attack;
    public int inactive_attack;

    public GameObject revive_lightning;
    public CameraShake cameraShake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        running_left = true;
        death_count = 2;
        just_died = false;
        death_time = 0;
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        FOXMAN_ATTACK = GetComponent<EnemyAttack>();
        FOXMAN_ATTACK.destroyTimer = 1.0f;

        starting_x = transform.position.x;
        max_move_range = 4;

        active_attack = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_attack = LayerMask.NameToLayer("Enemy");
    }
    // Update is called once per frame
    void Update()
    {
        if (is_dying_for_real && !just_died)
        {
            just_died = true;
            ani.SetBool("is_destroyed", true);
            FOXMAN_ATTACK.startDestroy();
        }

        if (health <= 0 && !just_died)
        {
            health = 0;
            death_time = 10f;
        }

        if (death_time > 0)
        {
            if (!just_died)
            {
                if (death_count > 0)
                {
                    death_count--;
                    just_died = true;
                }
                else
                {
                    MURDER_FOXMAN();
                    is_dying_for_real = true;
                }
                gameObject.layer = inactive_attack;
            }

            death_time -= Time.deltaTime;
            if (death_time <= 0)
            {
                death_time = 0;
                is_reviving = true;
                health = max_health;
                just_died = false;
                Instantiate(revive_lightning, new Vector2(transform.position.x, 7.5f), Quaternion.Euler(0, 0, 0));
                if (cameraShake)
                {
                    cameraShake.shakeOnHurt(0.8f);
                }
            }
        }

        if (standing_time > 0 && death_time == 0)
        {
            standing_time -= Time.deltaTime;
            if (standing_time <= 0)
            {
                if (running_left)
                {
                    running_left = false;
                }
                else
                {
                    running_left = true;
                }

                turning_around = false;
                standing_time = 0;
            }
        }

        ani.SetBool("is_dead", death_time > 0);
        ani.SetBool("is_reviving", is_reviving);

    }
    void FixedUpdate()
    {
        if (death_time > 0 || is_reviving || turning_around) {
            rb.linearVelocityX = 0;
            return;
        }

        current_position = transform.position;
        gameObject.layer = active_attack;

        if (running_left)
        {
            if (current_position.x <= starting_x - max_move_range)
            {
                if (!turning_around)
                {
                    turning_around = true;
                    standing_time = 1.5f;
                }
            }
            else
            {
                rb.linearVelocityX = -4.0f;
            }
        }

        else
        {
            if (current_position.x >= starting_x + max_move_range)
            {
                if (!turning_around)
                {
                    turning_around = true;
                    standing_time = 1.5f;
                }
            }
            else
            {
                rb.linearVelocityX = 4.0f;
            }
        }
    }
    public void MURDER_FOXMAN()
    {
        FOXMAN_ATTACK.DisableHitbox();
        FOXMAN_ATTACK.startDestroy();
    }
    public void Hit(PlayerAttack ouchies)
    {
        ouchies.isHit();
        TakeDamage(ouchies.damage);
    }
    public void TakeDamage(int damage)
    {
        if (is_reviving || death_time > 0)
        {
            Debug.Log("FOXMAN DYING?");
            return;
        }

        health -= damage;

    }
}
