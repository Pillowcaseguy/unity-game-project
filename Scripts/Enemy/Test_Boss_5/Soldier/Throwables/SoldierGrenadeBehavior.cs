using UnityEngine;

public class SoldierGrenadeBehavior : MonoBehaviour
{
    public int times_hit_ground = 0;
    public float moveTime;
    public bool doneMoving = false;
    public bool isExploding = false;

    public float timeSinceStopping = 0;

    public float groundHitCooldown = 0f;
    public float breakableGroundHitCooldown = 0f;
    public float peak_height;
    public float peak_speed;
    public bool peak_speed_negative;

    public GameObject explosion;

    public Rigidbody2D rb;

    LayerMask playerLayer;
    LayerMask groundLayer;
    LayerMask breakableGroundLayer;
    public SpriteRenderer sprite;

    //public bool iced;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        moveTime = 6f;
        playerLayer = LayerMask.GetMask("Player_Damage", "Player");
        groundLayer = LayerMask.GetMask("Ground");
        breakableGroundLayer = LayerMask.GetMask("BreakableGround");
    }

    // Update is called once per frame
    void Update()
    {
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

        if (breakableGroundHitCooldown > 0)
        {
            breakableGroundHitCooldown -= Time.deltaTime;
            if (breakableGroundHitCooldown <= 0)
            {
                breakableGroundHitCooldown = 0;
            }
        }

        if (doneMoving)
        {
            timeSinceStopping += Time.deltaTime;
            if (timeSinceStopping > 1.0f)
            {
                Explode();
            }
        }

        /*Vector2 rayStart = (Vector2)transform.position;
        Vector2 rayDirection = Vector2.left;
        float rayDistance = 0.5f;
        //Debug.DrawRay(rayStart, rayDirection, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDirection, rayDistance, breakableGroundLayer);

        if (hit.collider != null)
        {
            Explode();
        }*/

    }
    void FixedUpdate()
    {
        /*if (iced)
        {
            sprite.color = new Vector4(0, 0.6f, 1f, 1f);
            rb.linearVelocityX = 0;
            return;
        }*/

        if (rb.linearVelocityY > 0 && rb.linearVelocityY > peak_height)
        {
            peak_height = rb.linearVelocityY;
        }

        if (Mathf.Abs(rb.linearVelocityX) > peak_speed)
        {
            peak_speed = Mathf.Abs(rb.linearVelocityX);
            if (rb.linearVelocityX < 0)
            {
                peak_speed_negative = true;
            }
        }

        if (!doneMoving)
        {
            rb.rotation += 5;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            /*if (iced)
            {
                Destroy(gameObject);
                return;
            }*/

            if (!doneMoving && groundHitCooldown <= 0)
            {
                times_hit_ground += 1;
                if (Mathf.Abs(rb.linearVelocityX) < 0.1f)
                {
                    doneMoving = true;
                }
                else
                {
                    rb.linearVelocityX /= 1.2f;
                }
                peak_speed = 0;
                peak_speed_negative = false;
                if (!(Mathf.Abs(rb.linearVelocityX) < 0.1f))
                    rb.linearVelocityY = peak_height / (times_hit_ground * 0.9f);

                peak_height = 0;
                groundHitCooldown = 0.5f;
            }
            else if (doneMoving)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }

        if (((1 << col.gameObject.layer) & breakableGroundLayer) != 0)
        {
            /*if (iced)
            {
                Destroy(gameObject);
                return;
            }*/

            if (!doneMoving && breakableGroundHitCooldown <= 0)
            {
                breakableGroundHitCooldown = 0.5f;
                rb.linearVelocityX = (peak_speed * (peak_speed_negative ? -1 : 1)) * -1;
                peak_speed = 0;
                peak_speed_negative = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            //if (!iced)
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
        /*if (iced)
        {
            Destroy(gameObject);
            return;
        }*/
        isExploding = true;
        var hi = Instantiate(explosion, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.Euler(0, 0, 0));
        Destroy(hi, 0.45f);
        //float rot = 30;

        /*for (int i = 0; i < 10; i++)
        {
            GameObject frag = Instantiate(fragment, transform.position, Quaternion.Euler(0, 0, rot + 36 * i));
            FragmentBehavior fraggy = frag.GetComponent<FragmentBehavior>();
            fraggy.Initialize(6, rot + 36 * i, 1.5f);
        }*/

        Destroy(gameObject);
    }
}
