using UnityEngine;

public class LightParticleBehavior : MonoBehaviour
{
    public float lifetime;
    public float color_change_time;

    public LayerMask ground_layer;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        lifetime = 2.5f;
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        float x_force = Random.Range(-150f, 150f);
        float y_force = Random.Range(200f, 300f);

        rb.AddForce(new Vector2(x_force, y_force));
        rb.gravityScale = 1f;
        color_change_time = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
        color_change_time -= Time.deltaTime;
        if (color_change_time <= 0)
        {
            color_change_time = 0.15f;
            color_change();
        }
    }
    void color_change()
    {
        int new_color = Random.Range(0, 8);

        switch (new_color)
        {
            case 0:
                sprite.color = Color.red;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 1:
                sprite.color = Color.orange;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 2:
                sprite.color = Color.yellow;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 3:
                sprite.color = Color.green;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 4:
                sprite.color = Color.blue;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 5:
                sprite.color = Color.purple;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 6:
                sprite.color = Color.indigo;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            case 7:
                sprite.color = Color.white;
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.4f);
                break;
            default:
                break;
        }
    }
}
