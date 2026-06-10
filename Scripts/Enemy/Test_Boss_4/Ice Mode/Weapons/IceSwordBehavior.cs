using UnityEngine;

public class IceSwordBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public EnemyAttack attack;

    public float lifetime;
    public float speed;
    public bool initialized;
    public bool is_destroyed;
    public bool destroyed_first_time;

    public LayerMask ground_layer;
    public GameObject ice_particle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    public void initialize(float life, float sped)
    {
        lifetime = life;
        speed = sped;
        initialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        lifetime -= Time.deltaTime; 
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (!initialized)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }

        if (attack.wasHit || attack.wasBlocked || attack.wasParried)
        {
            is_destroyed = true;
        }

        if (!destroyed_first_time && is_destroyed)
        {
            destroyed_first_time = true;
            bool ground_stops = true;

            if (!(attack.wasHit || attack.wasBlocked || attack.wasParried))
            {
                ground_stops = false;
            }

            for (int x = 0; x < 50; x++)
            {
                if (ground_stops)
                {
                    Instantiate(ice_particle, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    var hi = Instantiate(ice_particle, gameObject.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<IceParticleBehavior>();
                    hi.keep_on_ground_hit = true;
                }
            }
            Destroy(gameObject);
        }

        if (!is_destroyed)
        {
            /*rb.linearVelocityX = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;*/
            rb.linearVelocityX = transform.right.x * speed;
            rb.linearVelocityY = transform.right.y * speed;
            //rb.rotation += (speed > 0) ? -10 : 10;
        }
    }
}
