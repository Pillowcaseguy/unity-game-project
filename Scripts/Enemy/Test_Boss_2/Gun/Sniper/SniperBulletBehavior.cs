using UnityEngine;
public class SniperBulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private SpriteRenderer sprite;
    private EnemyAttack enemyAttack;
    private Animator ani;
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

    public float impact_angle;
    public bool impact_angle_set = false;
    public float increment_movement_x;
    public float increment_movement_y;
    public float increment_rotation;

    public float halfHeight;
    public float rayLength;
    public Vector2 center;
    public Vector2 topOrigin;
    public Vector2 bottomOrigin;

    public GameObject bullet_deflection_effect;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground","BreakableGround");
        playerLayer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
        sprite = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponent<EnemyAttack>();
        ani = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        halfHeight = box.size.y * transform.localScale.y * 0.5f;
        rayLength = box.size.x * transform.localScale.x * 0.5f + 0.5f;
    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime, int bullet_type = 0)
    {
        speed = moveSpeed;
        rotation = zRotation;
        life = lifetime;
        type = bullet_type;

        if (type == 0) // Fire Bullet
        {
            enemyAttack.burnDuration = 8;
            enemyAttack.attackType = EnemyAttack.AttackType.Fire;
            sprite.color = Color.red;
        }
        else if (type == 1) // Ice Bullet
        {
            enemyAttack.freezeDuration = 2.5f;
            enemyAttack.attackType = EnemyAttack.AttackType.Ice;
            sprite.color = Color.lightBlue;
        }
        else if (type == 2) // Blind bullet (smoke)
        {
            enemyAttack.attackType = EnemyAttack.AttackType.Bullet;
            sprite.color = Color.white;
        }

        initialized = true;
    }
    private void do_destroy()
    {
        enemyAttack.DisableHitbox();
        enemyAttack.startDestroy();
    }
    public void bullet_flying()
    {
        if (!impact_angle_set)
        {
            impact_angle = transform.rotation.eulerAngles.z;
            impact_angle_set = true;
            Instantiate(bullet_deflection_effect,transform.position,Quaternion.Euler(0,0,0));

            center = box.bounds.center;
            topOrigin = center + (Vector2)(transform.up * halfHeight);
            bottomOrigin = center - (Vector2)(transform.up * halfHeight);

            /*Debug.DrawLine(center, center + (Vector2)(transform.right * rayLength), Color.blue);
            Debug.DrawLine(topOrigin, topOrigin + (Vector2)(transform.right * rayLength), Color.green);
            Debug.DrawLine(bottomOrigin, bottomOrigin + (Vector2)(transform.right * rayLength), Color.green);*/

            //RaycastHit2D attempt = Physics2D.Raycast(box_bounds, transform.right, box_size_x * transform.localScale.x * 0.5f + 1f, groundLayer);
            RaycastHit2D attempt;
            RaycastHit2D attempt1 = Physics2D.Raycast(center, transform.right, box.size.x * transform.localScale.x * 0.5f + 0.5f, groundLayer);
            RaycastHit2D attempt2 = Physics2D.Raycast(center + topOrigin, transform.right, rayLength, groundLayer);
            RaycastHit2D attempt3 = Physics2D.Raycast(center + bottomOrigin, transform.right, rayLength, groundLayer);

            if (attempt1.collider != null) attempt = attempt1;
            else if (attempt2.collider != null) attempt = attempt2;
            else if (attempt3.collider != null) attempt = attempt3;
            else attempt = attempt1;

            if (attempt.collider != null)
            {
                if (attempt.normal.x == -1)
                {
                    increment_movement_x = -0.3f;
                    increment_movement_y = -0.1f;
                    increment_rotation = 0f;
                    rotation = 180 - rotation;
                    transform.rotation = Quaternion.Euler(0, 0, rotation);
                }
                else if (attempt.normal.x == 1)
                {
                    increment_movement_x = 0.3f;
                    increment_movement_y = -0.1f;
                    increment_rotation = 0f;
                    rotation = 180 - rotation;
                    transform.rotation = Quaternion.Euler(0, 0, rotation);
                }
                else
                {
                    if (impact_angle > 180 && impact_angle < 270)
                    {
                        increment_movement_x = -0.3f;
                        increment_movement_y = 0.2f;
                        increment_rotation = 30f;
                    }
                    else if (impact_angle > 270 && impact_angle < 360)
                    {
                        increment_movement_x = 0.3f;
                        increment_movement_y = 0.2f;
                        increment_rotation = 30f;
                    }
                    rotation = -rotation;
                    transform.rotation = Quaternion.Euler(0, 0, rotation);
                }
            }
        }
        transform.position = new Vector2(transform.position.x + increment_movement_x, transform.position.y + increment_movement_y);
        transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z + increment_rotation);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.125f);
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.DrawLine(box.bounds.center, box.bounds.center + (transform.right * (box.size.x * transform.localScale.x * 0.5f + 0.15f)), Color.red);
        //Debug.DrawLine(box.bounds.center, box.bounds.center + (transform.right * (box.size.x * transform.localScale.x * 0.5f + 1f)), Color.blue);
        /*Debug.DrawLine(box.bounds.center, box.bounds.center + (transform.right * (box.size.y * transform.localScale.y * 0.5f + 1f)), Color.blue);
        Debug.DrawLine(box.bounds.center, box.bounds.center - (transform.right * (box.size.y * transform.localScale.y * 0.5f + 1f)), Color.blue);*/

        // Draw all three
        center = box.bounds.center;
        topOrigin = center + (Vector2)(transform.up * halfHeight);
        bottomOrigin = center - (Vector2)(transform.up * halfHeight);

        Debug.DrawLine(center, center + (Vector2)(transform.right * rayLength), Color.blue);
        Debug.DrawLine(topOrigin, topOrigin + (Vector2)(transform.right * rayLength), Color.green);
        Debug.DrawLine(bottomOrigin, bottomOrigin + (Vector2)(transform.right * rayLength), Color.green);

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
            if (!impact_angle_set)
            {
                if (type == 2 && !smoke_created)
                {
                    smoke_created = true;
                    GameObject smok = Instantiate(smoke, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
                    smok.GetComponent<SmokeBehavior>().initialize(10);
                }
                else if (type != 2)
                {
                    enemyAttack.destroyTimer = 1;
                }
                do_destroy();
                ani.SetBool("is_deflecting", true);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            if (type == 2 && col.gameObject.GetComponent<BreakableGroundBehavior>() != null)
            {
                smoke_created = true;
                GameObject smok = Instantiate(smoke, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
                smok.GetComponent<SmokeBehavior>().initialize(10);
                do_destroy();
            }
        }
    }
}
