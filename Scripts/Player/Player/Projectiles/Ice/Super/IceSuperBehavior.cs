using UnityEngine;

public class IceSuperBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerAttack playerAttack;
    public Animator ani;
    public float speed;
    public float initial_speed;
    public float rotation;
    public float life;

    private bool destroyed_first_time = false;
    private bool destroyed = false;

    private GameObject owner;
    private PlayerDamageAndHealth player_health;

    //public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
        ani = GetComponent<Animator>();
    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime, int number, GameObject target, GameObject creator, PlayerDamageAndHealth helf)
    {
        initial_speed = moveSpeed;
        speed = initial_speed / Time.timeScale;
        rotation = zRotation;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        life = lifetime;
        owner = creator;
        player_health = helf;
        if (target != null)
        {
            transform.position = target.transform.position + transform.up * (10 + number * 2);
        }
        else
        {
            float offset = player_health.movement.sprite_render.flipX ? -5 : 5;
            transform.position = new Vector2(creator.transform.position.x + offset, creator.transform.position.y) + new Vector2(transform.up.x,transform.up.y) * (10 + number * 2);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        ani.speed = 1 / Time.timeScale;
        if (life > 0f)
        {
            life -= Time.deltaTime;
            if (life < 0f)
            {
                Destroy(gameObject);
            }
        }

        if (!playerAttack.isDestroyed)
        {
            speed = Time.timeScale < 1f ? initial_speed / Time.timeScale : initial_speed * 2;
        }
    }
    private void FixedUpdate()
    {
        if (playerAttack.isDestroyed)
        {
            destroyed = true;
        }

        if (destroyed)
        {
            if (!destroyed_first_time)
            {
                destroyed_first_time = true;
                ani.SetBool("is_destroyed", true);
                playerAttack.DisableHitbox();
            }
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            speed = Time.timeScale < 1f ? initial_speed / Time.timeScale : initial_speed * 2;
            rb.linearVelocityY = -Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityX = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        }
    }
    public void destroyme()
    {
        Destroy(gameObject);
    }
    /*private void OnTriggerEnter2D(Collider2D col)
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
    }*/
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.layer != active_layer) // If the layer this object is on is not the active attack layer return
        {
            return;
        }

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
}
