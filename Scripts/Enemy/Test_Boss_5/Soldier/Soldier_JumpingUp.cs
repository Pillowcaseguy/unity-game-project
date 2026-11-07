using UnityEngine;

public class Soldier_JumpingUp : MonoBehaviour
{
    public Rigidbody2D rb;
    public TestBoss5Behavior_Soldier boss;
    public SpriteRenderer sprite;
    public Animator ani;

    public bool fine_to_destroy;
    public float lifetime;

    public bool traveling_left;
    public float soldier_x;

    public float speed;

    public bool slowing_down;
    public bool speeding_up;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        lifetime = 8f;
    }
    private void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            fine_to_destroy = true;
            sprite.enabled = false;
        }
        if (!traveling_left && transform.position.x >= 22)
        {
            fine_to_destroy = true;
            sprite.enabled = false;
        }
        else if (traveling_left && transform.position.x <= -22)
        {
            fine_to_destroy = true;
            sprite.enabled = false;
        }
        if (slowing_down)
        {
            rb.linearVelocityX -= traveling_left ? -0.1f : 0.1f;
            if (traveling_left && rb.linearVelocityX > 0f)
            {
                rb.linearVelocityX = 0f;
            }
            else if (!traveling_left && rb.linearVelocityX < 0f)
            {
                rb.linearVelocityX = 0f;
            }
        }
        else if (speeding_up)
        {
            rb.linearVelocityX += traveling_left ? -0.4f : 0.4f;
            if (traveling_left && rb.linearVelocityX < -4f)
            {
                rb.linearVelocityX = -4f;
            }
            else if (!traveling_left && rb.linearVelocityX > 4f)
            {
                rb.linearVelocityX = 4f;
            }
        }
    }
    public void confirmed_handshake()
    {
        if (traveling_left)
        {
            slowing_down = false;
            ani.SetBool("soldier_got_on",true);
            speeding_up = true;
        }
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}
