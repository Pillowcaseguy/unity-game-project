using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    public float speed;
    public float life;
    public Vector2 rotation;
    public bool initialized;

    private float height;
    private float width;
    private float half_width;
    private float half_height;

    private EnemyAttack attackControl;
    private bool destroyed_first_time = false;
    private bool destroyed = false;

    public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackControl = GetComponent<EnemyAttack>();
        groundLayer = LayerMask.GetMask("Ground","BreakableGround");
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        width = col.bounds.size.x;
        half_width = width / 2.0f;
        height = col.bounds.size.y;
        half_height = height / 2.0f;
    }
    public void Initialize(float moveSpeed, float lifetime)
    {
        speed = moveSpeed;
        life = lifetime;
        initialized = true;
        rotation = transform.right;
    }
    // Update is called once per frame
    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (rb.linearVelocityX < 0f)
        {
            sprite.flipX = true;
        }

        else
        {
            sprite.flipX = false;
        }

        if (life > 0f)
        {
            life -= Time.deltaTime;
            if (life < 0f)
            {
                Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!initialized)
        {
            return;
        }

        if (attackControl.isDestroyed)
        {
            destroyed = true;

        }

        if (destroyed)
        {
            if (!destroyed_first_time)
            {
                destroyed_first_time = true;
                attackControl.DisableHitbox();
                //transform.position = new Vector3(transform.position.x + Mathf.Cos(rotation * Mathf.Deg2Rad) * half_width, transform.position.y + Mathf.Sin(rotation * Mathf.Deg2Rad) * half_height, 0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }

            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;

        }

        else
        {
            //rb.linearVelocityX = Mathf.Cos(rotation.x * Mathf.Deg2Rad) * speed;
            rb.linearVelocityX = rotation.x * speed;
            //rb.linearVelocityY = Mathf.Sin(rotation.y * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = rotation.y * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            attackControl.DisableHitbox();
            attackControl.destroyTimer = 0.5f;
            attackControl.startDestroy();
        }
    }
}
