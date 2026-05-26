using UnityEngine;

public class SnowballBehavior : MonoBehaviour
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
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
        player_layer = LayerMask.GetMask("Player");
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
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
        if (!initialized || attack.isDestroyed)
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
            ani.SetBool("is_destroyed", true);
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            /*rb.linearVelocityX = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;*/
            rb.linearVelocityX = transform.right.x * speed;
            rb.linearVelocityY = transform.right.y * speed;
            sprite.transform.rotation = Quaternion.Euler(0, 0, sprite.transform.eulerAngles.z + 10);
            //rb.rotation += (speed > 0) ? -10 : 10;
        }
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
