using UnityEngine;

public class SadBombBehavior : MonoBehaviour, EnemyDamageable
{

    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Transform transform;

    public float starting_x;
    public float starting_y;
    public float target_x;
    public float target_y;

    public int random_position;
    public float lifetime;

    public bool hit_target_x;
    public bool hit_target_y;

    public bool donezos;

    public int health = 30;
    //public int health = 80;
    public float time_since_creation;

    public GameObject sad_wolf_man;

    public bool initialized;
    public void initialize(int random, float max_lifetime)
    {
        random_position = random;
        if (random_position == 1)
        {
            target_x = -4;
        }

        else if (random_position == 2)
        {
            target_x = -1;
        }

        else if (random_position == 3)
        {
            target_x = 2;
        }

        else
        {
            target_x = -4;
        }

        lifetime = max_lifetime;

        initialized = true;
    }
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        box = gameObject.GetComponent<BoxCollider2D>();
        starting_x = transform.position.x;
        starting_y = transform.position.y;
        target_y = 2;
        health = 60;
    }
    private void FixedUpdate()
    {
        if (donezos || !initialized)
        {
            return;
        }

        hit_target_x = Mathf.Abs(target_x - transform.position.x) <= 0.01;
        hit_target_y = Mathf.Abs(target_y - transform.position.y) <= 0.01;

        if (!hit_target_x)
        {
            bool negative = (target_x < starting_x);
            hit_target_x = negative ? (transform.position.x < target_x) : (transform.position.x > target_x);
        }

        if (!hit_target_y)
        {
            bool negative = (target_y < starting_y);
            hit_target_y = negative ? (transform.position.y < target_y) : (transform.position.y > target_y);
        }

        if (hit_target_x)
        {
            rb.linearVelocityX = 0;
        }
        else
        {
            rb.linearVelocityX = (target_x - starting_x) / 4;
        }

        if (hit_target_y)
        {
            rb.linearVelocityY = 0;
        }
        else
        {
            rb.linearVelocityY = (target_y - starting_y) / 4;
        }

        if (hit_target_x && hit_target_y)
        {
            donezos = true;
        }

    }
    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        time_since_creation += Time.deltaTime;

        if (time_since_creation >= lifetime)
        {
            Instantiate(sad_wolf_man, transform.position, transform.rotation);
            destroy();
        }

    }
    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isHit();
        TakeDamage(player_attack.damage);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            destroy();
        }
    }
    public void destroy()
    {
        Destroy(gameObject);
    }

}
