using UnityEngine;

public class NinjaDefeatedBehavior : MonoBehaviour
{
    public Animator ani;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public TestBoss5Behavior_Ninja boss;
    public LayerMask ground_layer;
    public bool hit_ground;
    public bool bounced;
    public bool onGround;
    void Awake()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground");
    }
    private void Start()
    {
        rb.AddForceX(sprite.flipX ? -250 : 250);
        rb.AddForceY(50);
    }
    public void dim_sprite()
    {
        sprite.color = new Vector4(1, 1, 1, sprite.color.a - 0.4f);
    }
    public void hit_ground_yes()
    {
        hit_ground = true;
    }
    void FixedUpdate()
    {
        boss.transform.position = gameObject.transform.position;
        if (transform.position.y <= -20f)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        else if (hit_ground && onGround)
        {
            ani.SetBool("hit_ground", true);
            rb.AddForce(new Vector2(sprite.flipX ? -100 : 100, 100));
            bounced = true;
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            onGround = true;
            if (bounced && onGround)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                boss.transform.position = gameObject.transform.position;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            onGround = false;
        }
    }
}
