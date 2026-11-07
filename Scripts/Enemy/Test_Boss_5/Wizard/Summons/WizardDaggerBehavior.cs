using UnityEngine;

public class WizardDaggerBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public EnemyAttack attack;

    public LayerMask ground_layer;

    public bool broken;
    public bool hit_ground;
    public bool hit_ground_first_time;
    public bool hit_ground_broken;
    public bool ricocheting;

    public GameObject crack;
    public GameObject crack_point;
    public GameObject gem_particle;

    public float particle_cooldown;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        attack = GetComponent<EnemyAttack>();
        ground_layer = LayerMask.GetMask("Ground");
    }
    void FixedUpdate()
    {
        float z = (transform.rotation.eulerAngles.z % 360 + 360) % 360;

        if (ricocheting)
        {
            if (rb.linearVelocityX > 0)
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, z - 8);
                rb.bodyType = RigidbodyType2D.Dynamic;
                if (ricocheting)
                {
                    sprite.color = new Vector4(1, 1, 1, sprite.color.a - 0.005f);
                }
            }
            else
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, z + 8);
                rb.bodyType = RigidbodyType2D.Dynamic;
                if (ricocheting)
                {
                    sprite.color = new Vector4(1, 1, 1, sprite.color.a - 0.005f);
                }
            }
        }

        if (!ricocheting && !hit_ground)
        {
            if (rb.linearVelocityX > 0)
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, z - 20);
            }
            else
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, z + 20);
            }
        }

        if (Mathf.Abs(transform.position.x) >= 20f || Mathf.Abs(transform.position.y) >= 10f)
        {
            Destroy(gameObject);
        }

        if (hit_ground && !hit_ground_first_time)
        {
            hit_ground_first_time = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            sprite.color = sprite.color = new Vector4(1, 1, 1, 0.8f);
            sprite.sortingOrder = -6;
            Instantiate(crack, new Vector2(crack_point.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardGroundCrackBehavior>().time_before_fade = 999;
        }

        if (sprite.color.a <= 0)
        {
            Destroy(gameObject);
        }

        particle_cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            attack.DisableHitbox();
            if (!broken && !ricocheting && ((transform.rotation.eulerAngles.z % 360 + 360) % 360 > 345 || (transform.rotation.eulerAngles.z % 360 + 360) % 360 < 91))
            {
                hit_ground = true;
            }
            else if (!hit_ground && !broken)
            {
                rb.linearVelocityY = Mathf.Max(-rb.linearVelocityY, 0);
                ricocheting = true;
                if (particle_cooldown <= 0)
                {
                    for (int i = 0; i < Random.Range(10, 20); i++)
                    {
                        Instantiate(gem_particle, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.Euler(0, 0, 0));
                    }
                    particle_cooldown = 2f;
                }
            }

            if (broken)
            {
                rb.linearVelocityY = Mathf.Max(-rb.linearVelocityY / 2f, 0);
                ricocheting = true;
                if (particle_cooldown <= 0)
                {
                    for (int i = 0; i < Random.Range(10, 20); i++)
                    {
                        Instantiate(gem_particle, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.Euler(0, 0, 0));
                    }
                    particle_cooldown = 2f;
                }
            }
        }
    }
}
