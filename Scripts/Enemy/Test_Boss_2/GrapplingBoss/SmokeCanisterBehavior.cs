using UnityEngine;

public class SmokeCanisterBehavior : MonoBehaviour
{
    public float lifetime = 4.0f;

    public bool isExploding = false;

    public GameObject smoke;
    public GameObject contact_point;
    public bool hit_something = false;

    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    LayerMask playerLayer;
    LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask("Player_Damage", "Player");
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        if (contact_point == null)
        {
            sprite.transform.Rotate(0, 0, -5);
        }

        if (contact_point != null && !hit_something)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 270);
            hit_something = true;
            rb.gravityScale = 0;
            rb.linearVelocityY = 0;
        }
    }

    void Update()
    {
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
        GameObject hi = Instantiate(smoke, new Vector2(transform.position.x,transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
        hi.GetComponent<SmokeBehavior>().initialize(20);
        Destroy(gameObject);
    }
}
