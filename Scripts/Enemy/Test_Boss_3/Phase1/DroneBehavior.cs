using UnityEngine;

public class DroneBehavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{

    public int health;
    public int maximum_health = 100;
    public float stop_time;
    public float move_time;
    public Vector2 current_goal;
    public float lifetime;

    public GameObject player;
    public bool initialized = false;

    public Rigidbody2D rb;
    public Animator ani;

    public bool is_breaking;
    public bool is_exploding;

    public int playerLayer;

    public EnemyAttack enemy_attack;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        health = maximum_health;
        stop_time = 0.2f;
        playerLayer = LayerMask.GetMask("Player_Damage");
        lifetime = 20f;
    }
    public void initialize(GameObject playa)
    {
        player = playa;
        initialized = true;
    }
    void Update()
    {
        if (!initialized || is_breaking || is_exploding)
        {
            ani.SetBool("is_breaking", is_breaking);
            ani.SetBool("is_exploding", is_exploding);
            return;
        }

        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                is_exploding = true;
            }
        }

        if (move_time <= 0 && stop_time <= 0)
        {
            move_time = Random.Range(1.5f, 2f);
            current_goal = player.transform.position;
        }

        if (move_time > 0)
        {
            move_time -= Time.deltaTime;
            if (move_time <= 0)
            {
                stop_time = 0.2f;
            }
        }

        if (stop_time > 0)
        {
            stop_time -= Time.deltaTime;
            if (stop_time <= 0)
            {
                move_time = Random.Range(1.5f,2f);
                current_goal = player.transform.position;
            }
        }
    }
    void FixedUpdate()
    {
        if (!initialized)
        {
            return;
        }

        if (stop_time > 0 || is_exploding)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }

        if (health <= 0 && !is_breaking)
        {
            rb.linearVelocityY = 0;
            rb.linearVelocityX = 0;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 4;
            stop_time = 0;
            move_time = 0;
            is_breaking = true;
        }

        if (is_exploding || is_breaking)
        {
            return;
        }

        if (move_time > 0)
        {
            if (current_goal.x > transform.position.x)
            {
                rb.linearVelocityX = Random.Range(2.0f,3f);
            }
            else
            {
                rb.linearVelocityX = Random.Range(-2.0f, -3f);
            }
            if (current_goal.y > transform.position.y)
            {
                rb.linearVelocityY = Random.Range(2.0f,3f);
            }
            else
            {
                rb.linearVelocityY = Random.Range(-2.0f, -3f);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (health <= 0)
        {
            return;
        }
        health -= damage;
    }
    
    public void Hit(PlayerAttack player_attack)
    {
        if (is_exploding || is_breaking)
        {
            return;
        }

        player_attack.isHit();
        TakeDamage(player_attack.damage);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (is_breaking || is_exploding)
        {
            return;
        }

        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            is_exploding = true;
        }
    }
    public void activate_hitbox()
    {
        enemy_attack.gameObject.layer = LayerMask.NameToLayer("Active_Enemy_Attack");
    }
    public void destroy(float time = 0f)
    {
        if (is_breaking)
        {
            gameObject.layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
        }
        Destroy(gameObject, time);
    }
}
