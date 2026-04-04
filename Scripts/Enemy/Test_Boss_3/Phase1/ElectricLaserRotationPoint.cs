using UnityEngine;

public class ElectricLaserRotationPoint : MonoBehaviour
{

    public Rigidbody2D rb;
    public bool can_move = false;
    public bool not_attacking = false;
    public int rotation_step = 1;
    public float move_cooldown;
    public float move_wait_time;

    public SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (not_attacking && can_move)
        {
            can_move = false;
        }
        if (!can_move)
        {
            if (not_attacking)
            {
                sprite.enabled = false;
            }
            move_cooldown = move_wait_time;
            return;
        }
        move_cooldown -= Time.fixedDeltaTime;
        if (move_cooldown < 0)
        {
            move_cooldown = move_wait_time;
            rb.rotation += rotation_step;
        }
    }
}
