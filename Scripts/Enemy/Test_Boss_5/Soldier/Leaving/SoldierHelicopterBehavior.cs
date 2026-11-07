using UnityEngine;

public class SoldierHelicopterBehavior : MonoBehaviour
{
    public Animator ani;
    public Animator ladder_ani;

    public SpriteRenderer sprite;
    public SpriteRenderer ladder_sprite;

    public Rigidbody2D rb;
    public TestBoss5Behavior_Soldier boss;

    public float target_x;
    public bool traveling_left;

    public bool reached_destination;
    public bool reached_destination_first_time;

    public bool fine_to_destroy;
    public float lifetime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifetime = 10f;
    }
    void FixedUpdate()
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

        if (traveling_left)
        {
            if (transform.position.x <= target_x + 2 && !reached_destination_first_time)
            {
                ani.SetBool("slowing", true);
                reached_destination_first_time = true;
                Invoke("boss_gets_on", 1.5f);
            }
        }
        else if (!traveling_left)
        {
            if (transform.position.x >= target_x - 2 && !reached_destination_first_time)
            {
                ani.SetBool("slowing", true);
                reached_destination_first_time = true;
                Invoke("boss_gets_on", 1.5f);
            }
        }

        if (ani.GetBool("slowing"))
        {
            ladder_ani.SetBool("stopped", true);
            rb.linearVelocityX -= traveling_left ? -0.4f : 0.4f;
            if (traveling_left && rb.linearVelocityX > 0f)
            {
                rb.linearVelocityX = 0f;
            }
            else if (!traveling_left && rb.linearVelocityX < 0f)
            {
                rb.linearVelocityX = 0f;
            }
        }
        else if (!ani.GetBool("slowing"))
        {
            rb.linearVelocityX += traveling_left ? -0.4f : 0.4f;
            if (traveling_left && rb.linearVelocityX < -12f)
            {
                rb.linearVelocityX = -12f;
            }
            else if (!traveling_left && rb.linearVelocityX > 12f)
            {
                rb.linearVelocityX = 12f;
            }
        }
    }
    public void boss_gets_on()
    {
        ani.SetBool("slowing", false);
        ani.SetBool("picked_soldier_up", true);
        ladder_ani.SetBool("soldier_got_on", true);
        reached_destination = true;
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}
