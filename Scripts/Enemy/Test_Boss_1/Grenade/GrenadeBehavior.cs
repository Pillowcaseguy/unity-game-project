using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{

    public int times_hit_ground = 0;
    public float xVel;
    public float yVel;
    public float moveTime = 8f;
    public bool doneMoving = false;
    public bool isExploding = false;

    public bool firstThrown = true;
    public float initial_force_x = 0;
    public float initial_force_y = 0;

    private bool initialized = false;

    public float timeSinceStopping = 0;

    public float groundHitCooldown = 0.5f;

    public GameObject explosion;
    public GameObject fragment;

    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Transform transform;

    LayerMask playerLayer;
    LayerMask groundLayer;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        transform = GetComponent<Transform>();

        playerLayer = LayerMask.GetMask("Player_Damage","Player");
        groundLayer = LayerMask.GetMask("Ground");
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

        if (moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            if (moveTime <= 0)
            {
                doneMoving = true;
            }
        }

        if (groundHitCooldown > 0)
        {
            groundHitCooldown -= Time.deltaTime;
            if (groundHitCooldown <= 0)
            {
                groundHitCooldown = 0;
            }
        }

        if (doneMoving)
        {
            timeSinceStopping += Time.deltaTime;
        }

        if (timeSinceStopping > 1.0f)
        {
            Explode();
        }

        Vector2 rayStart = (Vector2)transform.position;
        Vector2 rayDirection = Vector2.left;
        float rayDistance = 0.5f;
        //Debug.DrawRay(rayStart, rayDirection, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDirection, rayDistance, groundLayer);

        if (hit.collider != null) {
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
            //rb.AddForce(new Vector2(-150, 200));
            rb.AddForce(new Vector2(initial_force_x, initial_force_y));
            firstThrown = false;
        }

        if (doneMoving)
        {
            rb.linearVelocityX = 0;
        }

        else
        {
            rb.rotation += 5;
        }

        if (!doneMoving)
        {
            if (xVel != 0)
            {
                rb.linearVelocityX = xVel;
                xVel = 0;
            }

            if (yVel != 0)
            {
                rb.linearVelocityY = yVel;
                yVel = 0;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            if (!doneMoving && groundHitCooldown == 0) {
                times_hit_ground += 1;
                xVel = -1.8f;
                yVel = 6.0f / times_hit_ground;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            Explode();
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
        float rot = 30;

        for (int i = 0; i < 10; i++)
        {
            GameObject frag = Instantiate(fragment, transform.position, Quaternion.Euler(0, 0, rot + 36 * i));
            FragmentBehavior fraggy = frag.GetComponent<FragmentBehavior>();
            fraggy.Initialize(6, rot + 36 * i, 1.5f);
        }

        Destroy(gameObject);
    }
}
