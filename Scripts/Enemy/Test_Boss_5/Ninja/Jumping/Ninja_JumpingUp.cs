using UnityEngine;

public class Ninja_JumpingUp : MonoBehaviour
{
    public Rigidbody2D rb;
    public TestBoss5Behavior_Ninja boss;
    public SpriteRenderer sprite;

    public bool fine_to_destroy;
    public float lifetime;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        lifetime = 3f;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            fine_to_destroy = true;
            sprite.enabled = false;
        }
        if (transform.position.y >= 10)
        {
            fine_to_destroy = true;
            sprite.enabled = false;
        }
    }
    public void do_jump()
    {
        float x_force = -transform.position.x * 70;
        rb.AddForce(new Vector2(x_force, 900));
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}
