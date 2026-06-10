using UnityEngine;

public class FireSuperBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerAttack playerAttack;
    public Animator ani;
    public float initial_speed;
    public float speed;
    public float life;

    public bool attack_fired;

    public float player_x;
    public float player_y;

    private bool destroyed_first_time = false;
    private bool destroyed = false;

    private GameObject owner;
    private PlayerDamageAndHealth player_health;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        ani = GetComponent<Animator>();
    }
    public void Initialize(float moveSpeed, float lifetime, GameObject creator, PlayerDamageAndHealth helf)
    {
        initial_speed = moveSpeed;
        speed = initial_speed / Time.timeScale;
        life = lifetime;
        owner = creator;
        player_x = creator.transform.position.x;
        player_y = creator.transform.position.y;
        player_health = helf;
        transform.position = new Vector2(creator.transform.position.x - 10, creator.transform.position.y + 10);
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
                do_attack();
                Destroy(gameObject);
            }
        }

        if (!playerAttack.isDestroyed)
        {
            speed = Time.timeScale < 1f ? initial_speed / Time.timeScale : initial_speed;
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
            speed = Time.timeScale < 1f ? initial_speed / Time.timeScale : initial_speed;
            rb.linearVelocityX = Mathf.Cos(315 * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(315 * Mathf.Deg2Rad) * speed;
        }
    }
    public void destroyme()
    {
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (!(transform.position.x >= player_x && transform.position.y <= player_y))
        {
            return;
        }
        if (((1 << col.gameObject.layer) & groundLayer) != 0) // If the collision is hitting the ground layer
        {
            do_attack();
        }
    }
    public void do_attack()
    {
        if (!attack_fired)
        {
            attack_fired = true;
            Camera cam = Camera.main;
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;

            var hi = cam.GetComponent<CameraShake>();
            if (hi != null)
            {
                hi.shakeOnHurt(1f);
            }

            Collider2D[] hits = Physics2D.OverlapBoxAll(cam.transform.position, new Vector2(width, height), 0f, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                EnemyDamageable target = hit.GetComponent<EnemyDamageable>();
                if (target == null) continue;
                EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
                if (further_target == null) continue;
                target.Hit(playerAttack);
            }
            playerAttack.isDestroyed = true;
        }
    }
}
