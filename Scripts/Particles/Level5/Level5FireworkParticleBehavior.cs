using UnityEngine;

public class Level5FireworkParticleBehavior : MonoBehaviour
{
    public float existence_time;

    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        existence_time = 0.5f;

        float x_force = Random.Range(-5f, 5f);
        float y_force = Random.Range(-3f, -1f);

        rb.linearVelocity = new Vector2(x_force, y_force);
    }
    private void FixedUpdate()
    {
        existence_time -= Time.deltaTime;
        if (existence_time <= 0)
        {
            rb.linearVelocityX *= Random.Range(0.96f, 0.98f);
            rb.linearVelocityY *= Random.Range(0.98f, 0.99f);
        }
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 1.5f / 255f);
        if (sprite.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}