using UnityEngine;

public class WizardFireworkBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator ani;

    public GameObject particle;

    public float lifetime;
    public bool exploded;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        lifetime = 6f;
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 270);

        if (transform.position.y >= 4.2f && !exploded)
        {
            explode();
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            explode();
        }
    }
    public void explode()
    {
        if (!exploded)
        {
            rb.linearVelocity = Vector2.zero;
            exploded = true;
            ani.SetBool("exploding", true);
        }
    }
    public void set_random_color()
    {
        int random_color = Random.Range(0, 5);

        if (random_color == 0) // Blue
        {
            sprite.color = new Vector4(0.3f, 0.7f, 1f, 0.8f);
        }
        else if (random_color == 1) // Red
        {
            sprite.color = new Vector4(1, 0, 0, 0.8f);
        }
        else if (random_color == 2) // Orange
        {
            sprite.color = new Vector4(1, 0.5f, 0, 0.8f);
        }
        else if (random_color == 3) // Yellow
        {
            sprite.color = new Vector4(1, 1, 0, 0.8f);
        }
        else if (random_color == 4) // Purple
        {
            sprite.color = new Vector4(0.6f, 0.3f, 1f, 0.8f);
        }
    }
    public void destroy_me()
    {
        for (int i = 0; i < 25; i++)
        {
            var hi = Instantiate(particle, new Vector2(transform.position.x + Random.Range(-0.35f, 0.35f), transform.position.y + Random.Range(-1.3f, -1.05f)), Quaternion.Euler(0, 0, 0))
                .GetComponent<Level5FireworkParticleBehavior>();
            hi.sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        }
        Destroy(gameObject);
    }
}
