using UnityEngine;

public class CrystalBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public EnemyAttack attack;
    public Animator ani;
    public float speed;
    public float rotation;
    public float life;
    public bool facing_left;

    private bool destroyed_first_time = false;
    private bool destroyed = false;

    private GameObject owner;
    private GameObject target;

    public LayerMask groundLayer;
    //public LayerMask enemyLayer;
    public LayerMask player_layer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;
    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        //enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
        player_layer = LayerMask.GetMask("Player_Damage");
        ani = GetComponent<Animator>();
    }
    public void Initialize(float moveSpeed, float lifetime, float the_rotation, float offset, bool facing_leftwards, GameObject the_target, GameObject creator)
    {
        speed = moveSpeed;
        rotation = the_rotation;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        life = lifetime;
        facing_left = facing_leftwards;
        target = the_target;
        owner = creator;
        if (target != null)
        {
            float first_offset = 15;
            float off = facing_left ? -first_offset : first_offset;
            transform.position = target.transform.position + transform.up * off;
            transform.position = new Vector2(transform.position.x, transform.position.y + offset);
        }
        else
        {
            transform.position = new Vector2(creator.transform.position.x + 5, creator.transform.position.y) + new Vector2(transform.up.x, transform.up.y) * 10;
        }
    }
    // Update is called once per frame
    private void Update()
    {
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
        if (attack.isDestroyed)
        {
            destroyed = true;
        }

        if (destroyed)
        {
            if (!destroyed_first_time)
            {
                destroyed_first_time = true;
                ani.SetBool("is_destroyed", true);
                float fix = facing_left ? 180 : 0;
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + fix);
                attack.DisableHitbox();
            }
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            rb.linearVelocityX = -Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            if (col.gameObject.CompareTag("Doesn'tBlockProjectiles"))
                return;
            //bool check_pass_1 = facing_left ? (transform.position.x <= target.transform.position.x) : (transform.position.x >= target.transform.position.x);
            if (transform.position.y <= target.transform.position.y)
            {
                attack.startDestroy();
            }
        }
    }
}
