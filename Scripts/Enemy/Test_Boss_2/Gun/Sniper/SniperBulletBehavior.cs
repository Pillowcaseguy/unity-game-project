using UnityEngine;
public class SniperBulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private EnemyAttack enemyAttack;
    public float speed;
    public float rotation;
    public float life;
    public bool initialized = false;

    public int type;
    public GameObject smoke;
    public bool smoke_created = false;

    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground","BreakableGround");
        playerLayer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
        sprite = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponent<EnemyAttack>();
    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime, int bullet_type = 0)
    {
        speed = moveSpeed;
        rotation = zRotation;
        life = lifetime;
        type = bullet_type;

        if (type == 1) // Fire Bullet
        {
            enemyAttack.burnDuration = 8;
            enemyAttack.attackType = EnemyAttack.AttackType.Fire;
            sprite.color = Color.red;
        }
        else if (type == 2) // Ice Bullet
        {
            enemyAttack.freezeDuration = 2.5f;
            enemyAttack.attackType = EnemyAttack.AttackType.Ice;
            sprite.color = Color.lightBlue;
        }
        else if (type == 3) // Blind bullet (smoke)
        {
            enemyAttack.attackType = EnemyAttack.AttackType.Bullet;
            sprite.color = Color.white;
        }
        else
        {
            enemyAttack.attackType = EnemyAttack.AttackType.Bullet;
            sprite.color = Color.yellow;
        }

        initialized = true;
    }
    private void do_destroy()
    {
        enemyAttack.DisableHitbox();
        enemyAttack.startDestroy();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!initialized)
        {
            return;
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

        if (!enemyAttack.isDestroyed)
        {
            rb.linearVelocityX = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        }
        else
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            if (type == 3 && !smoke_created)
            {
                smoke_created = true;
                GameObject smok = Instantiate(smoke, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
                smok.GetComponent<SmokeBehavior>().initialize(10);
            }
            do_destroy();
        }

        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            if (type == 3 && !smoke_created)
            {
                smoke_created = true;
                GameObject smok = Instantiate(smoke, new Vector2 (transform.position.x,transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
                smok.GetComponent<SmokeBehavior>().initialize(10);
            }
            do_destroy();
        }
    }
}
