using UnityEngine;

public class IceBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    public PlayerAttack playerAttack;
    public EnemyAttack reflectAttack;
    public float speed;
    public float rotation;
    public float life;

    private bool destroyed_first_time = false;
    private bool destroyed = false;

    public bool reflected_first_time = false;
    public bool reflected = false;
    private GameObject owner;
    private PlayerDamageAndHealth player_health;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    public bool hit_legit_target;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        reflectAttack = GetComponent<EnemyAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground","BreakableGround");
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack","Enemy");
        playerLayer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime, GameObject creator, PlayerDamageAndHealth helf)
    {
        speed = moveSpeed;
        rotation = zRotation;
        life = lifetime;
        owner = creator;
        player_health = helf;
    }
    // Update is called once per frame
    private void Update()
    {
        if (reflected)
        {
            if (!reflected_first_time)
            {
                reflected_first_time = true;
                transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + 180);
                rotation = transform.rotation.eulerAngles.z;
            }
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
        if (playerAttack.isDestroyed || reflectAttack.isDestroyed)
        {
            destroyed = true;
        }

        if (destroyed)
        {
            if (!destroyed_first_time)
            {
                if (playerAttack.wasHit && hit_legit_target)
                {
                    player_health.AddEnergy(Mathf.RoundToInt(playerAttack.damage * 0.8f));
                }
                destroyed_first_time = true;
                GetComponentInChildren<IceTrailBehavior>().disableSprite();
                playerAttack.DisableHitbox();
                transform.rotation = Quaternion.Euler(0, 0, rotation);
            }

            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;

        }
        else
        {
            rb.linearVelocityX = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Reflective") && ((1 << col.gameObject.layer) & enemyLayer) != 0)
        {
            reflected = true;
        }
        else if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            playerAttack.DisableHitbox();
            playerAttack.destroyTimer = 0.5f;
            playerAttack.startDestroy();
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.layer != active_layer) // If the layer this object is on is not the active attack layer return
        {
            return;
        }

        if (!reflected)
        {
            if (((1 << col.gameObject.layer) & enemyLayer) != 0) // If the collision is hitting the enemy's damage hitbox layer
            {
                EnemyDamageable attempt = col.gameObject.GetComponent<EnemyDamageable>();
                if (attempt == null)
                {
                    return;
                }
                else
                {
                    if (player_health != null && player_health.health > 0)
                    {
                        hit_legit_target = true;
                        attempt.Hit(playerAttack);
                    }
                    else
                    {
                        playerAttack.damage = 0;
                        attempt.Hit(playerAttack);
                    }
                }   
            }
        }
        else
        {
            if (((1 << col.gameObject.layer) & playerLayer) != 0) // If the collision is hitting the player's damage hitbox layer
            {
                col.gameObject.GetComponentInParent<PlayerDamageAndHealth>().Hit(this.col);
            }
        }
    }
}
