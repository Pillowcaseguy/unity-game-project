using UnityEngine;

public class MiddleFireballBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator ani;
    public SpriteRenderer sprite;
    public EnemyAttack attack;

    public bool initialized;
    public float lifetime;
    public float speed;

    public LayerMask player_layer;
    public LayerMask ground_layer;

    public float swing;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponentInChildren<EnemyAttack>();
        player_layer = LayerMask.GetMask("Player");
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    public void initialize(float life, float sped, float the_rotation, float swung = 0)
    {
        lifetime = life;
        speed = sped;
        transform.rotation = Quaternion.Euler(0, 0, the_rotation);
        swing = swung;
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
            return;
            //sprite.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
        }

        if (attack.isDestroyed)
        {
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            ani.SetBool("is_destroyed", true);
            attack.DisableHitbox();
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
            destroyme();
        }
        else
        {
            /*rb.linearVelocityX = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;*/
            rb.linearVelocityX = attack.gameObject.transform.right.x * speed;
            rb.linearVelocityY = attack.gameObject.transform.right.y * speed;
            attack.gameObject.transform.rotation = Quaternion.Euler(0, 0, attack.gameObject.transform.eulerAngles.z + swing);
            //sprite.transform.rotation = Quaternion.Euler(0, 0, sprite.transform.eulerAngles.z + 10);
            //rb.rotation += (speed > 0) ? -10 : 10;
        }
    }
    public void destroyme()
    {
        Destroy(gameObject);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (attack.isDestroyed)
        {
            return;
        }

        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            attack.startDestroy();
        }

        /*if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            PlayerDamageAndHealth attempt = col.gameObject.GetComponent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                attempt.Hit_No_Collider(gameObject);
            }
        }*/
    }
}
