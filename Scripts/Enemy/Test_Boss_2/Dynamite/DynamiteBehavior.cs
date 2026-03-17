using UnityEngine;

public class DynamiteBehavior : MonoBehaviour
{

    public float lifetime = 8.0f;

    public bool firstThrown = true;
    public float xVel;
    public float yVel;
    public bool isExploding = false;

    public float initial_force_x = 0;
    public float initial_force_y = 0;

    private bool initialized = false;

    public GameObject explosion;

    private Rigidbody2D rb;
    private BoxCollider2D box;
    private EnemyAttack attack;

    LayerMask playerLayer;
    LayerMask groundLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponentInChildren<BoxCollider2D>();
        attack = GetComponentInChildren<EnemyAttack>();

        playerLayer = LayerMask.GetMask("Player_Damage", "Player");
        groundLayer = LayerMask.GetMask("Ground","BreakableGround");
    }
    public void Initialize(float int_x, float int_y)
    {
        initialized = true;
        rb.gravityScale = 0.5f;
        initial_force_x = int_x;
        initial_force_y = int_y;
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
            Explode();
        }

    }
    void FixedUpdate()
    {
        if (!initialized)
        {
            rb.gravityScale = 0f;
            return;
        }

        if (firstThrown)
        {
            rb.AddForce(new Vector2(initial_force_x, initial_force_y));
            firstThrown = false;
        }

        rb.rotation += 5;

    }

    private void OnTriggerStay2D(Collider2D col)
    {

        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            BreakableGroundBehavior attempt = col.gameObject.GetComponent<BreakableGroundBehavior>();
            if (attempt != null)
            {
                attempt.breakGround();
            }

            Explode();
        }

        else if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            if (attack.wasHit || attack.wasParried || attack.wasBlocked)
            {
                Explode();
            }
        }
    }
    public void Explode()
    {
        if (isExploding)
        {
            return;
        }
        isExploding = true;
        Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
