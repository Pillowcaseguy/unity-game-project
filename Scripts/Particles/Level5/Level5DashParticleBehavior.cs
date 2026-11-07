using UnityEngine;

public class Level5DashParticleBehavior : MonoBehaviour
{
    public float lifetime;
    public float existence_time;
    //public bool keep_on_ground_hit;

    //public LayerMask ground_layer;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        int random_color = Random.Range(0, 5);

        if (random_color == 0)
        {

        }
        else if (random_color == 1) // Red
        {
            sprite.color = new Vector4(1, 0, 0, 0.4f);
        }
        else if (random_color == 2) // Orange
        {
            sprite.color = new Vector4(1, 0.5f, 0, 0.25f);
        }
        else if (random_color == 3) // Yellow
        {
            sprite.color = new Vector4(1, 1, 0, 0.25f);
        }
        else if (random_color == 4)
        {

        }
        lifetime = 2.5f;
        existence_time = 0.5f;
        //ground_layer = LayerMask.GetMask("Ground", "BreakableGround");

        float x_force = Random.Range(-200f, 200f);
        float y_force = Random.Range(-200f, 200f);

        rb.AddForce(new Vector2(x_force, y_force));
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        existence_time -= Time.deltaTime;
        if (existence_time <= 0)
        {
            rb.linearVelocityX *= 0.95f;
            rb.linearVelocityY *= 0.95f;
        }
    }
    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (!keep_on_ground_hit && ((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            Destroy(gameObject);
        }
    }*/
}