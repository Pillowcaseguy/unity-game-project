using UnityEngine;

public class LandmineBehavior : MonoBehaviour
{

    public float lifetime = 8.0f;

    public bool isExploding = false;

    public GameObject explosion;
    public GameObject contact_point;
    public bool hit_something = false;

    public Rigidbody2D rb;
    public BoxCollider2D box;
    public SpriteRenderer sprite;
    public Animator ani;

    public float half_width;
    public float scale;
    public float half_height;

    LayerMask playerLayer;
    LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        playerLayer = LayerMask.GetMask("Player_Damage", "Player");
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        rb.linearVelocityY = 6;
        sprite = GetComponentInChildren<SpriteRenderer>();
        ani = GetComponentInChildren<Animator>();

        scale = 3;
        half_width = box.size.x / 2f * scale;
        half_height = box.size.y / 2f * scale;
    }
    private void FixedUpdate()
    {
        if (contact_point == null)
        {
            sprite.transform.Rotate(0,0,10);
        }

        if (contact_point != null && !hit_something)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            hit_something = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocityY = 0;
            ani.SetBool("is_active", true);
        }
    }

    void Update()
    {
        Debug.DrawLine(new Vector2(transform.position.x - half_width + 0.1f,transform.position.y - half_height),
            new Vector2(transform.position.x - half_width + 0.1f, transform.position.y - half_height - 0.1f), Color.red);
        Debug.DrawLine(new Vector2(transform.position.x + half_width - 0.1f,transform.position.y - half_height),
            new Vector2(transform.position.x + half_width - 0.1f, transform.position.y - half_height - 0.1f), Color.red);
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                lifetime = 0;
                Explode();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (((1 << col.gameObject.layer) & groundLayer) != 0 && contact_point == null)
        {
            contact_point = col.gameObject;
        }

        else if (((1 << col.gameObject.layer) & playerLayer) != 0)
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
        if (contact_point != null)
        {
            BreakableGroundBehavior attempt = contact_point.GetComponent<BreakableGroundBehavior>();
            if (attempt != null)
            {
                attempt.breakGround();
            }
        }
        else
        {
            RaycastHit2D attempt1 = Physics2D.Raycast(new Vector2(transform.position.x + half_width - 0.1f, transform.position.y - half_height - 0.1f), -transform.up, 2f, groundLayer);
            RaycastHit2D attempt2 = Physics2D.Raycast(new Vector2(transform.position.x - half_width + 0.1f,transform.position.y - half_height - 0.1f), -transform.up, 2f, groundLayer);
            GameObject first_hit = null;
            if (attempt1.collider != null)
            {
                first_hit = attempt1.collider.gameObject;
                BreakableGroundBehavior bg = first_hit.GetComponent<BreakableGroundBehavior>();
                if (bg != null)
                {
                    bg.breakGround();
                }
            }
            if (attempt2.collider != null)
            {
                bool same_thing = first_hit == attempt2.collider.gameObject;
                BreakableGroundBehavior bgg = attempt2.collider.gameObject.GetComponent<BreakableGroundBehavior>();
                if (bgg != null && !same_thing)
                {
                    bgg.breakGround();
                }
            }
            
        }
        Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
