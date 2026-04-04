using UnityEngine;

public class LaserProjectileBehavior : MonoBehaviour
{
    public EnemyAttack my_attack;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public int speed;
    public bool is_destroyed;
    public bool destroy_now;
    public GameObject laser_impact;
    public GameObject laser_impact_spot;

    public bool initialized;

    public int ground_layer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        my_attack = GetComponent<EnemyAttack>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    public void initialize(int s)
    {
        speed = s;
        initialized = true;
    }
    private void FixedUpdate()
    {
        if (initialized)
        {
            rb.linearVelocityX = transform.right.x * speed;
            rb.linearVelocityY = transform.right.y * speed;
        }
    }
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (my_attack.wasParried)
        {
            destroy_now = true;
            destroy();
        }

        if (my_attack.isDestroyed && !is_destroyed)
        {
            destroy();
        }
    }
    void destroy()
    {
        if (destroy_now)
        {
            is_destroyed = true;
            Destroy(gameObject);
        }
        if (!is_destroyed)
        {
            is_destroyed = true;
            Instantiate(laser_impact, laser_impact_spot.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            destroy();
        }
    }
}
