using UnityEngine;

public class SoldierBulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public Animator ani;
    public BoxCollider2D col;
    public float speed;
    public float life;
    public Vector2 rotation;
    public bool initialized;

    private float height;
    private float width;
    private float half_width;
    private float half_height;
    public bool stopped;

    private EnemyAttack attackControl;

    public LayerMask groundLayer;
    public LayerMask player_layer;

    public LevelManager_TestBoss5 manager;
    public bool high_shot;

    public float starting_x;
    public float starting_boss_x;
    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackControl = GetComponent<EnemyAttack>();
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        player_layer = LayerMask.GetMask("Player_Damage");
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        width = col.bounds.size.x;
        half_width = width / 2.0f;
        height = col.bounds.size.y;
        half_height = height / 2.0f;
    }
    public void Initialize(float moveSpeed, float lifetime, bool high_attack, LevelManager_TestBoss5 manage, float boss_x)
    {
        speed = moveSpeed;
        life = lifetime;
        initialized = true;
        rotation = transform.right;
        high_shot = high_attack;
        manager = manage;
        starting_boss_x = boss_x;
        starting_x = manager.player.transform.position.x;
    }
    // Update is called once per frame
    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (high_shot)
        {
            if (manager.player_life.movement.crouching && Mathf.Abs(starting_x - starting_boss_x) < 10)
            {
                attackControl.DisableHitbox();
            }
            else
            {
                attackControl.EnableHitbox();
            }
        }
        else
        {
            if (!manager.player_life.movement.onGround && !manager.player_life.movement.dashing && Mathf.Abs(starting_x - starting_boss_x) < 10)
            {
                attackControl.DisableHitbox();
            }
            else
            {
                attackControl.EnableHitbox();
            }
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
        //rb.linearVelocityX = Mathf.Cos(rotation.x * Mathf.Deg2Rad) * speed;
        //rb.linearVelocityY = Mathf.Sin(rotation.y * Mathf.Deg2Rad) * speed;
        if (!ani.GetBool("is_destroyed"))
        {
            rb.linearVelocityX = rotation.x * speed;
            rb.linearVelocityY = rotation.y * speed;
        }
        else
        {
            if (!stopped)
            {
                transform.position = new Vector2(transform.position.x - rotation.x * width, transform.position.y - Mathf.Sin(rotation.y * Mathf.Deg2Rad) * height);
                rb.linearVelocity = Vector2.zero;
                stopped = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            ani.SetBool("is_destroyed", true);
            attackControl.DisableHitbox();
        }
        if (((1 << col.gameObject.layer) & player_layer) != 0 && gameObject.layer == attackControl.active_layer && !manager.player_life.player_died)
        {
            ani.SetBool("is_destroyed", true);
        }
    }
    public void grow_size()
    {
        transform.localScale = new Vector2(3, 3);
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}
