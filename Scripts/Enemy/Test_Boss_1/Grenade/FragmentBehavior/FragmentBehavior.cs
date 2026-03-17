using UnityEngine;

public class FragmentBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private EnemyAttack enemyAttack;
    public float speed;
    public float rotation;
    public float life;

    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
        col = GetComponent<BoxCollider2D>();
        enemyAttack = GetComponent<EnemyAttack>();

    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime)
    {
        speed = moveSpeed;
        rotation = zRotation;
        life = lifetime;
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
        rb.linearVelocityX = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
        rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            enemyAttack.DisableHitbox();
            enemyAttack.destroyTimer = 0.0f;
            enemyAttack.startDestroy();
        }

    }

}

